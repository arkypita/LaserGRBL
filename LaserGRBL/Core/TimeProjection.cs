using System;
using System.Collections.Generic;
using System.Linq;
using Sound;

namespace LaserGRBL
{
    /// <summary>
    /// Tracks job progress and provides time estimates for single and multi-pass jobs.
    /// </summary>
    public class TimeProjection
    {
        private readonly Func<long> _timeProvider;

        private TimeSpan mETarget;
        private TimeSpan mEProgress;

        private long mStart;        //Start Time
        private long mEnd;          //End Time
        private long mGlobalStart;  //Global Start (multiple pass)
        private long mGlobalEnd;    //Global End (multiple pass)
        private long mPauseBegin;   //Pause begin Time
        private long mCumulatedPause;

        private bool mInPause;
        private bool mCompleted;
        private bool mStarted;

        private int mTargetCount;
        private int mExecutedCount;
        private int mSentCount;
        private int mErrorCount;
        private int mContinueCorrection;

        private GrblCore.DetectedIssue mLastIssue;
        private GPoint mLastKnownWCO;

        // Pass tracking for improved multi-pass estimates
        private List<TimeSpan> mCompletedPassTimes = new List<TimeSpan>();
        private TimeSpan mOriginalEstimate = TimeSpan.Zero;
        private int mTotalPasses = 1;  // Total number of passes for the job

        public GPoint LastKnownWCO
        {
            get { return mLastKnownWCO; }
            set { if (InProgram) mLastKnownWCO = value; }
        }

        public TimeProjection() : this(() => Tools.HiResTimer.TotalMilliseconds)
        {
        }

        public TimeProjection(Func<long> timeProvider)
        {
            _timeProvider = timeProvider ?? throw new ArgumentNullException(nameof(timeProvider));
            Reset(true);
        }

        public void Reset(bool global)
        {
            mETarget = TimeSpan.Zero;
            mEProgress = TimeSpan.Zero;
            mStart = mEnd = 0;
            if (global)
            {
                mGlobalStart = mGlobalEnd = 0;
                // Clear pass history on global reset
                mCompletedPassTimes.Clear();
                mOriginalEstimate = TimeSpan.Zero;
            }
            mPauseBegin = 0;
            mCumulatedPause = 0;
            mInPause = false;
            mCompleted = false;
            mStarted = false;
            mExecutedCount = 0;
            mSentCount = 0;
            mErrorCount = 0;
            mTargetCount = 0;
            mContinueCorrection = 0;
            mLastIssue = GrblCore.DetectedIssue.Unknown;
            mLastKnownWCO = GPoint.Zero;
        }

        public TimeSpan EstimatedTarget
        { get { return mETarget; } }

        public bool InProgram
        { get { return mStarted && !mCompleted; } }

        public int Target
        { get { return mTargetCount; } }

        public int Sent
        { get { return mSentCount - mContinueCorrection; } }

        public int Executed
        { get { return mExecutedCount - mContinueCorrection; } }

        private DateTime mLastLogTime = DateTime.MinValue;

        public TimeSpan ProjectedTarget
        {
            get
            {
                if (mStarted)
                {
                    double real = TrueJobTime.TotalSeconds; //job time spent in execution
                    double target = mETarget.TotalSeconds;  //total estimated
                    double done = mEProgress.TotalSeconds;  //done of estimated

                    double projected;

                    if (done == 0)
                    {
                        // No progress yet, use the target estimate
                        if ((DateTime.Now - mLastLogTime).TotalSeconds >= 5)
                        {
                            mLastLogTime = DateTime.Now;
                        }
                        return EstimatedTarget;
                    }

                    // Check if estimated progress exceeds target (estimate was wrong)
                    // In that case, use command count instead of time estimate
                    bool estimateOverrun = done > target;
                    double commandProgress = mTargetCount > 0 ? (double)mExecutedCount / mTargetCount : 0;

                    if (estimateOverrun && commandProgress > 0)
                    {
                        // Time estimate is unreliable, use actual command progress
                        projected = real / commandProgress;
                    }
                    // If we have completed passes, use historical data intelligently
                    else if (mCompletedPassTimes.Count > 0)
                    {
                        double avgPassTime = mCompletedPassTimes.Average(t => t.TotalSeconds);
                        double progressPercent = done / target;

                        if (progressPercent < 0.2)
                        {
                            // Less than 20% progress - trust historical average
                            projected = avgPassTime;
                        }
                        else
                        {
                            // Blend historical data with current trajectory
                            // Early on, trust history more; later, trust current trajectory more
                            double trajectoryEstimate = real * target / done;
                            double historyWeight = Math.Max(0, 1.0 - progressPercent);  // 80% at 20%, 0% at 100%
                            double trajectoryWeight = progressPercent;

                            projected = (avgPassTime * historyWeight) + (trajectoryEstimate * trajectoryWeight);
                        }
                    }
                    else
                    {
                        // No historical data, use current trajectory
                        projected = real * target / done;
                    }

                    return TimeSpan.FromSeconds(projected) + TotalJobPauses;
                }
                else
                    return TimeSpan.Zero;
            }
        }

        public TimeSpan ProjectedTimeRemaining
        {
            get
            {
                if (mStarted && !mCompleted)
                {
                    TimeSpan projected = ProjectedTarget;
                    TimeSpan elapsed = TotalJobTime;
                    TimeSpan remaining = projected - elapsed;
                    return remaining > TimeSpan.Zero ? remaining : TimeSpan.Zero;
                }
                return TimeSpan.Zero;
            }
        }

        public TimeSpan ProjectedTotalTime
        {
            get
            {
                if (mStarted)
                {
                    TimeSpan singlePassProjection = ProjectedTarget;
                    int remainingPasses = mTotalPasses - mCompletedPassTimes.Count;
                    if (mStarted && !mCompleted)
                        remainingPasses--; // Current pass is not in completed yet

                    // Include time remaining for current pass plus all remaining future passes
                    TimeSpan currentPassRemaining = ProjectedTimeRemaining;
                    TimeSpan futurePassesTime = TimeSpan.FromTicks(singlePassProjection.Ticks * Math.Max(0, remainingPasses));
                    TimeSpan totalTime = TotalGlobalJobTime + currentPassRemaining + futurePassesTime;

                    if ((DateTime.Now - mLastLogTime).TotalSeconds >= 2)
                    {
                        mLastLogTime = DateTime.Now;
                        int commandsRemaining = mTargetCount - mExecutedCount;
                        double progressPercent = mETarget.TotalSeconds > 0 ? (mEProgress.TotalSeconds / mETarget.TotalSeconds) * 100 : 0;

                        Logger.LogMessage("TimeProjection", "Progress: {0}/{1} commands ({2:F0}%), EstimatedProgress: {3:F1}s/{4:F1}s ({5:F0}%)",
                            mExecutedCount, mTargetCount, ((double)mExecutedCount / mTargetCount) * 100,
                            mEProgress.TotalSeconds, mETarget.TotalSeconds, progressPercent);
                        Logger.LogMessage("TimeProjection", "ProjectedTotalTime - Elapsed: {0:F1}s, CurrentRemaining: {1:F1}s, FuturePasses({2}): {3:F1}s, Total: {4:F1}s",
                            TotalGlobalJobTime.TotalSeconds, currentPassRemaining.TotalSeconds, remainingPasses,
                            futurePassesTime.TotalSeconds, totalTime.TotalSeconds);
                    }

                    return totalTime;
                }
                return TimeSpan.Zero;
            }
        }

        public TimeSpan ProjectedTotalTimeRemaining
        {
            get
            {
                if (mStarted)
                {
                    TimeSpan projectedTotal = ProjectedTotalTime;
                    TimeSpan elapsed = TotalGlobalJobTime;
                    TimeSpan remaining = projectedTotal - elapsed;
                    return remaining > TimeSpan.Zero ? remaining : TimeSpan.Zero;
                }
                return TimeSpan.Zero;
            }
        }

        private TimeSpan TrueJobTime
        { get { return TotalJobTime - TotalJobPauses; } }

        public TimeSpan TotalJobTime
        {
            get
            {
                if (mCompleted)
                    return TimeSpan.FromMilliseconds(mEnd - mStart);
                else if (mStarted)
                    return TimeSpan.FromMilliseconds(now - mStart);
                else
                    return TimeSpan.Zero;
            }
        }

        public TimeSpan TotalGlobalJobTime
        {
            get
            {
                if (mCompleted)
                    return TimeSpan.FromMilliseconds(mGlobalEnd - mGlobalStart);
                else if (mStarted)
                    return TimeSpan.FromMilliseconds(now - mGlobalStart);
                else
                    return TimeSpan.Zero;
            }
        }

        private TimeSpan TotalJobPauses
        {
            get
            {
                if (mInPause)
                    return TimeSpan.FromMilliseconds(mCumulatedPause + (now - mPauseBegin));
                else
                    return TimeSpan.FromMilliseconds(mCumulatedPause);
            }
        }

        public void JobStart(GrblFile file, Queue<GrblCommand> mQueuePtr, bool global, int totalPasses = 1)
        {
            if (!mStarted)
            {
                // Store total passes count
                if (global)
                    mTotalPasses = totalPasses;

                int currentPassNumber = mCompletedPassTimes.Count + 1;
                Logger.LogMessage("TimeProjection", "JobStart - Pass {0}/{1}, Global={2}", currentPassNumber, mTotalPasses, global);

                // Store original estimate on first pass
                if (mOriginalEstimate == TimeSpan.Zero)
                    mOriginalEstimate = file.EstimatedTime;

                // Use actual pass times from previous passes to improve estimates
                if (mCompletedPassTimes.Count > 0)
                {
                    // Use the average of completed passes as the estimate
                    // This provides a much more accurate estimate than the original calculation
                    double totalSeconds = 0;
                    foreach (var passTime in mCompletedPassTimes)
                        totalSeconds += passTime.TotalSeconds;
                    double avgSeconds = totalSeconds / mCompletedPassTimes.Count;
                    mETarget = TimeSpan.FromSeconds(avgSeconds);
                    Logger.LogMessage("TimeProjection", "Using average of {0} completed passes: {1:F1}s (original estimate was {2:F1}s)",
                        mCompletedPassTimes.Count, avgSeconds, mOriginalEstimate.TotalSeconds);
                }
                else
                {
                    // First pass - use file's estimated time
                    mETarget = file.EstimatedTime;
                    Logger.LogMessage("TimeProjection", "First pass - using file estimate: {0:F1}s", mETarget.TotalSeconds);
                }

                mTargetCount = mQueuePtr.Count;
                mEProgress = TimeSpan.Zero;
                mStart = _timeProvider();
                if (global) mGlobalStart = mStart;
                mPauseBegin = 0;
                mInPause = false;
                mCompleted = false;
                mStarted = true;
                mExecutedCount = 0;
                mSentCount = 0;
                mErrorCount = 0;
                mContinueCorrection = 0;
                mLastIssue = GrblCore.DetectedIssue.Unknown;
                mLastKnownWCO = GPoint.Zero;
            }
        }

        public void JobContinue(GrblFile file, int position, int added)
        {
            if (!mStarted)
            {
                if (mETarget == TimeSpan.Zero) mETarget = file.EstimatedTime;
                if (mTargetCount == 0) mTargetCount = file.Count;
                //mEProgress = TimeSpan.Zero;
                if (mStart == 0)
                    mGlobalStart = mStart = _timeProvider();

                mPauseBegin = 0;
                mInPause = false;
                mCompleted = false;
                mStarted = true;
                mExecutedCount = position;
                mSentCount = position;
                mLastIssue = GrblCore.DetectedIssue.Unknown;
                //	mErrorCount = 0;
                mContinueCorrection = added;
            }
        }

        public void JobSent()
        {
            if (mStarted && !mCompleted)
                mSentCount++;
        }

        public void JobError()
        {
            if (mStarted && !mCompleted)
            {
                SoundEvent.PlaySound(SoundEvent.EventId.Warning);
                mErrorCount++;
            }
        }

        public void JobExecuted(TimeSpan EstimatedProgress)
        {
            if (mStarted && !mCompleted)
            {
                mExecutedCount++;
                mEProgress = EstimatedProgress;
            }
        }

        public void JobPause()
        {
            if (mStarted && !mCompleted && !mInPause)
            {
                mInPause = true;
                mPauseBegin = now;
            }
        }

        public void JobResume()
        {
            if (mStarted && !mCompleted && mInPause)
            {
                mCumulatedPause += _timeProvider() - mPauseBegin;
                mInPause = false;
            }
        }

        public bool JobEnd(bool global)
        {
            if (mStarted && !mCompleted)
            {
                JobResume(); //nel caso l'ultimo comando fosse una pausa, la chiudo e la cumulo
                mEnd = _timeProvider();

                // Store actual pass time (excluding pauses) for future estimates
                // This is critical for multi-pass jobs - do this BEFORE handling global flag
                TimeSpan actualPassTime = TrueJobTime;
                if (actualPassTime > TimeSpan.Zero)
                {
                    mCompletedPassTimes.Add(actualPassTime);
                    Logger.LogMessage("TimeProjection", "JobEnd - Pass {0}/{1} completed in {2:F1}s (estimate was {3:F1}s), Global={4}",
                        mCompletedPassTimes.Count, mTotalPasses, actualPassTime.TotalSeconds, mETarget.TotalSeconds, global);

                    // Send notification on second pass start (first pass just completed)
                    if (!global && mCompletedPassTimes.Count == 1 && mTotalPasses > 1 && Ntfy.SecondPassEnabled && Ntfy.Enabled)
                    {
                        // Check if total estimated time meets threshold
                        TimeSpan projectedTotal = ProjectedTotalTime;
                        
                        if (projectedTotal.TotalMinutes >= Ntfy.Threshold)
                        {
                            TimeSpan remaining = ProjectedTotalTimeRemaining;
                            DateTime completionTime = DateTime.Now.Add(remaining);
                            string message = string.Format("Starting Pass 2/{0} - ETC: {1} - Total Job Time: {2}",
                                mTotalPasses,
                                completionTime.ToString("h:mm tt"),
                                FormatTimeSpan(projectedTotal));

                            Ntfy.NotifySecondPass(projectedTotal, message);
                        }
                    }
                }

                if (global)
                {
                    mGlobalEnd = mEnd;
                    Logger.LogMessage("TimeProjection", "JobEnd - Global end, total time: {0:F1}s, {1} passes completed",
                        TotalGlobalJobTime.TotalSeconds, mCompletedPassTimes.Count);
                }

                mCompleted = true;
                mStarted = false;
                return true;
            }

            return false;
        }

        private static string FormatTimeSpan(TimeSpan ts)
        {
            if (ts.TotalHours >= 1)
                return string.Format("{0}h {1}m", (int)ts.TotalHours, ts.Minutes);
            else if (ts.TotalMinutes >= 1)
                return string.Format("{0}m {1}s", (int)ts.TotalMinutes, ts.Seconds);
            else
                return string.Format("{0}s", (int)ts.TotalSeconds);
        }

        public void JobIssue(GrblCore.DetectedIssue issue)
        { mLastIssue = issue; }

        private long now
        { get { return _timeProvider(); } }

        public int ErrorCount
        { get { return mErrorCount; } }

        public GrblCore.DetectedIssue LastIssue
        { get { return mLastIssue; } }
    }
}
