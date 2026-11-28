using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Windows.Forms;

namespace LaserGRBL.DeviceProfiles
{
    internal static class JobPreview
    {
        internal static void Show(IWin32Window parent, DeviceProfile profile, IList<ExportMove> moves)
        {
            using (var dialog = new Form())
            {
                dialog.Text = "Profiiliekspordi eelvaade";
                dialog.StartPosition = FormStartPosition.CenterParent;
                dialog.ClientSize = new Size(760, 640);
                dialog.MinimumSize = new Size(440, 360);
                dialog.AutoScaleMode = AutoScaleMode.Font;
                dialog.MinimizeBox = false;
                dialog.ShowInTaskbar = false;

                var layout = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 3, ColumnCount = 1, Padding = new Padding(12) };
                layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
                layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
                layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

                var plot = new WorkArea(profile, moves);
                var dimensions = new Label
                {
                    AutoSize = true,
                    Text = string.Format(CultureInfo.CurrentCulture, "{0}  ·  {1:0.###} × {2:0.###} mm  ·  Võrk {3:0.###} mm",
                        profile.Name, profile.Width, profile.Height, plot.GridSpacing),
                };
                var showTravel = new CheckBox { AutoSize = true, Text = "Kuva liikumised väljalülitatud laseriga" };
                showTravel.CheckedChanged += (sender, args) => plot.SetTravelVisible(showTravel.Checked);
                var close = new Button { AutoSize = true, Anchor = AnchorStyles.Right, Text = "Sulge", DialogResult = DialogResult.Cancel };

                var controls = new TableLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, RowCount = 1, ColumnCount = 2 };
                controls.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
                controls.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
                controls.Controls.Add(showTravel, 0, 0);
                controls.Controls.Add(close, 1, 0);
                layout.Controls.Add(dimensions, 0, 0);
                layout.Controls.Add(plot, 0, 1);
                layout.Controls.Add(controls, 0, 2);
                dialog.Controls.Add(layout);
                dialog.CancelButton = close;
                dialog.ShowDialog(parent);
            }
        }

        private sealed class WorkArea : Control
        {
            private static readonly Color[] PowerColors =
            {
                Color.FromArgb(155, 171, 187), Color.FromArgb(122, 145, 171), Color.FromArgb(91, 119, 151),
                Color.FromArgb(64, 96, 131), Color.FromArgb(36, 68, 106), Color.FromArgb(14, 40, 76),
            };

            private readonly DeviceProfile profile;
            private readonly IList<ExportMove> moves;
            private bool showTravel;

            internal readonly decimal GridSpacing;

            internal WorkArea(DeviceProfile profile, IList<ExportMove> moves)
            {
                this.profile = profile;
                this.moves = moves;
                GridSpacing = GridInterval(Math.Max(profile.Width, profile.Height));
                Dock = DockStyle.Fill;
                BackColor = SystemColors.Control;
                DoubleBuffered = true;
                ResizeRedraw = true;
                AccessibleName = "Laseri tööala millimeetrites";
            }

            internal void SetTravelVisible(bool visible)
            {
                showTravel = visible;
                Invalidate();
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
                const int left = 52;
                const int top = 20;
                const int right = 35;
                const int bottom = 35;
                if (ClientSize.Width <= left + right || ClientSize.Height <= top + bottom)
                    return;

                float plotWidth = ClientSize.Width - left - right;
                float plotHeight = ClientSize.Height - top - bottom;
                float scale = Math.Min(plotWidth / (float)profile.Width, plotHeight / (float)profile.Height);
                float width = (float)profile.Width * scale;
                float height = (float)profile.Height * scale;
                var bed = new RectangleF(left + (plotWidth - width) / 2, top + (plotHeight - height) / 2, width, height);

                DrawGrid(e.Graphics, bed, scale);
                DrawMoves(e.Graphics, bed, scale);
            }

            private void DrawGrid(Graphics graphics, RectangleF bed, float scale)
            {
                graphics.FillRectangle(SystemBrushes.Window, bed);
                var zero = bed.Left;
                graphics.DrawString("0", Font, SystemBrushes.ControlText, zero - 4, bed.Bottom + 4);
                graphics.DrawString("X", Font, SystemBrushes.ControlText, bed.Right + 5, bed.Bottom + 4);
                graphics.DrawString("Y", Font, SystemBrushes.ControlText, bed.Left - 17, bed.Top - 14);

                for (decimal x = GridSpacing; x < profile.Width; x += GridSpacing)
                {
                    float pixel = bed.Left + (float)x * scale;
                    graphics.DrawLine(SystemPens.ControlLight, pixel, bed.Top, pixel, bed.Bottom);
                    string label = x.ToString("0.###", CultureInfo.CurrentCulture);
                    float labelWidth = graphics.MeasureString(label, Font).Width;
                    graphics.DrawString(label, Font, SystemBrushes.ControlText, pixel - labelWidth / 2, bed.Bottom + 4);
                }
                for (decimal y = GridSpacing; y < profile.Height; y += GridSpacing)
                {
                    float pixel = bed.Bottom - (float)y * scale;
                    graphics.DrawLine(SystemPens.ControlLight, bed.Left, pixel, bed.Right, pixel);
                    string label = y.ToString("0.###", CultureInfo.CurrentCulture);
                    SizeF labelSize = graphics.MeasureString(label, Font);
                    graphics.DrawString(label, Font, SystemBrushes.ControlText, bed.Left - labelSize.Width - 4, pixel - labelSize.Height / 2);
                }

                graphics.DrawRectangle(SystemPens.ControlDark, bed.Left, bed.Top, bed.Width, bed.Height);
            }

            private void DrawMoves(Graphics graphics, RectangleF bed, float scale)
            {
                var previous = new PointF(bed.Left, bed.Bottom);
                bool hasPrevious = moves.Count > 0 && moves[0].Rapid;
                var powers = new Pen[PowerColors.Length];
                GraphicsState state = graphics.Save();
                try
                {
                    graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    graphics.SetClip(bed);
                    for (int i = 0; i < powers.Length; i++)
                        powers[i] = new Pen(PowerColors[i], 1.35f);

                    using (var travel = new Pen(Color.DimGray, 1f) { DashStyle = DashStyle.Dot })
                    {
                        foreach (ExportMove move in moves)
                        {
                            var current = new PointF(bed.Left + (float)move.X * scale, bed.Bottom - (float)move.Y * scale);
                            if (hasPrevious && !move.Rapid && move.Power > 0)
                            {
                                int level = (int)((long)move.Power * (powers.Length - 1) / profile.MaxPower);
                                graphics.DrawLine(powers[level], previous, current);
                            }
                            else if (hasPrevious && showTravel)
                            {
                                graphics.DrawLine(travel, previous, current);
                            }

                            previous = current;
                            hasPrevious = true;
                        }
                    }
                }
                finally
                {
                    graphics.Restore(state);
                    foreach (Pen power in powers)
                        power?.Dispose();
                }
            }

            private static decimal GridInterval(decimal longestSide)
            {
                decimal target = longestSide / 10;
                decimal step = 1;
                while (target >= step * 10)
                    step *= 10;
                if (target >= 5 * step)
                    return 5 * step;
                return target >= 2 * step ? 2 * step : step;
            }
        }
    }
}
