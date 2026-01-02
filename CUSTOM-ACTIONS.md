# Custom Job Completed Action

LaserGRBL Storer Edition supports running a custom script when a job completes successfully.

## How to Configure

1. Create a PowerShell script (`.ps1` file) with the actions you want to perform
2. Open LaserGRBL Settings (File menu or toolbar)
3. Go to the **Sound & Notification** tab
4. Check **"Run custom script on job completion"**
5. Click **Browse...** to select your PowerShell script
6. Click **Save**

The script will now run automatically whenever a job completes successfully.

## Example Script

See [job-completed-example.ps1](job-completed-example.ps1) for an example script that:
- Logs each job completion with timestamp
- Shows how to add custom notifications
- Can be extended to control external devices, upload files, etc.

## Testing

1. Copy `job-completed-example.ps1` to `job-completed.ps1`
2. Configure the setting to point to your script
3. Run a test job (can be a simple short program)
4. When complete, check `job-completions.log` for the logged entry

## Future Enhancements

Future versions will add:
- UI for configuring the script path in Settings
- Passing job information (duration, file name, errors) to the script
- Support for multiple scripts or actions
- Conditional execution based on job success/failure
