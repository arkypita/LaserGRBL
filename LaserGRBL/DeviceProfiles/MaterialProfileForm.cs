using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows.Forms;
using System.Xml;

namespace LaserGRBL.DeviceProfiles
{
	internal sealed class MaterialProfileForm : Form
	{
		private readonly string folder;
		private readonly string deviceName;
		private readonly int maxPower;
		private readonly ListBox profiles = new ListBox { Dock = DockStyle.Fill, IntegralHeight = false };
		private readonly TextBox details = new TextBox { Multiline = true, ReadOnly = true, ScrollBars = ScrollBars.Vertical, Dock = DockStyle.Fill };
		private readonly Button edit = new Button { Text = "Muuda", AutoSize = true };
		private readonly Button remove = new Button { Text = "Eemalda", AutoSize = true };
		private readonly Button use = new Button { Text = "Kasuta materjali", AutoSize = true };
		private MaterialProfile selected;

		internal static MaterialProfile Choose(IWin32Window parent, string settingsFolder, int maxPower, string deviceName)
		{
			using (var dialog = new MaterialProfileForm(settingsFolder, maxPower, deviceName))
			{
				if (TryAction(parent, dialog.Text, () => dialog.Reload(Guid.Empty)) && dialog.ShowDialog(parent) == DialogResult.OK)
					return dialog.selected;
				return null;
			}
		}

		internal static string GetWarning(MaterialProfile profile, MaterialOperationKind? operation = null)
		{
			return MaterialProfiles.GetWarning(profile, operation);
		}

		private MaterialProfileForm(string settingsFolder, int powerMaximum, string device)
		{
			folder = settingsFolder;
			deviceName = device;
			maxPower = powerMaximum;
			Text = "Materjaliprofiilid · " + deviceName;
			ClientSize = new Size(715, 320);
			MinimumSize = new Size(610, 300);
			StartPosition = FormStartPosition.CenterParent;
			ShowInTaskbar = false;
			AutoScaleMode = AutoScaleMode.Font;

			var layout = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(12), ColumnCount = 2, RowCount = 2 };
			layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 36));
			layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 64));
			layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
			layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
			profiles.AccessibleName = "Saadaolevad materjaliprofiilid";
			profiles.DisplayMember = nameof(MaterialListEntry.Name);
			profiles.SelectedIndexChanged += (sender, args) => UpdateSelection();
			profiles.DoubleClick += (sender, args) => UseSelected();
			details.AccessibleName = "Valitud materjali seaded ja hoiatused";
			layout.Controls.Add(profiles, 0, 0);
			layout.Controls.Add(details, 1, 0);

			var actions = new FlowLayoutPanel { AutoSize = true, Dock = DockStyle.Fill, WrapContents = false };
			var add = new Button { Text = "Uus", AutoSize = true };
			var cancel = new Button { Text = "Loobu", AutoSize = true, DialogResult = DialogResult.Cancel };
			add.Click += (sender, args) => ShowEditor(null);
			edit.Click += (sender, args) => ShowEditor(SelectedProfile);
			remove.Click += (sender, args) => RemoveSelected();
			use.Click += (sender, args) => UseSelected();
			actions.Controls.AddRange(new Control[] { add, edit, remove, use, cancel });
			layout.Controls.Add(actions, 0, 1);
			layout.SetColumnSpan(actions, 2);
			Controls.Add(layout);
			AcceptButton = use;
			CancelButton = cancel;
		}

		private MaterialProfile SelectedProfile => (profiles.SelectedItem as MaterialListEntry)?.Profile;

		private void Reload(Guid selection)
		{
			profiles.DataSource = MaterialProfiles.Load(folder, deviceName, maxPower).Select(profile => new MaterialListEntry(profile)).ToArray();
			for (int index = 0; index < profiles.Items.Count; index++)
			{
				if (((MaterialListEntry)profiles.Items[index]).Profile.Id == selection)
				{
					profiles.SelectedIndex = index;
					break;
				}
			}
			UpdateSelection();
		}

		private void UpdateSelection()
		{
			MaterialProfile profile = SelectedProfile;
			edit.Enabled = remove.Enabled = profile != null && !profile.BuiltIn;
			use.Enabled = profile != null && !profile.IsUnsuitable;
			if (profile == null)
			{
				details.Text = "Vali või loo materjaliprofiil.";
				return;
			}
			string warning = MaterialProfiles.GetWarning(profile);
			details.Lines = new[]
			{
				Describe("Graveeri", profile.Engrave),
				Describe("Voltimisjoon", profile.Score),
				Describe("Lõika", profile.Cut),
				"", profile.Notes, warning
			}.Where(line => line != null).ToArray();
		}

		private static string Describe(string name, MaterialOperation operation)
		{
			if (operation == null)
				return name + "  ·  Seaded puuduvad";
			return string.Format("{0}  ·  S{1}, {2:0.###} mm/min, läbimisi {3}, {4}", name,
				operation.Power, operation.Feed, operation.Passes, operation.Confirmed ? "katsetatud" : "katsetamata");
		}

		private void UseSelected()
		{
			MaterialProfile profile = SelectedProfile;
			if (profile == null || profile.IsUnsuitable)
				return;
			selected = profile;
			DialogResult = DialogResult.OK;
		}

		private void ShowEditor(MaterialProfile profile)
		{
			if (profile != null && profile.BuiltIn)
				return;
			using (var editor = new MaterialEditor(profile, deviceName, maxPower, saved => MaterialProfiles.Save(folder, saved, profile != null)))
			{
				if (editor.ShowDialog(this) == DialogResult.OK)
					TryAction(() => Reload(editor.MaterialId));
			}
		}

		private void RemoveSelected()
		{
			MaterialProfile profile = SelectedProfile;
			if (profile == null || profile.BuiltIn || MessageBox.Show(this, "Eemaldada materjaliprofiil " + profile.Name + "?",
				Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
				return;
			TryAction(() =>
			{
				MaterialProfiles.Remove(folder, profile);
				Reload(Guid.Empty);
			});
		}

		private bool TryAction(Action action)
		{
			return TryAction(this, Text, action);
		}

		private static bool TryAction(IWin32Window parent, string caption, Action action)
		{
			try
			{
				action();
				return true;
			}
			catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException || ex is SerializationException ||
				ex is XmlException || ex is InvalidOperationException || ex is InvalidDataException)
			{
				MessageBox.Show(parent, ex.Message, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
		}

		private sealed class MaterialListEntry
		{
			internal MaterialListEntry(MaterialProfile profile) { Profile = profile; }
			internal MaterialProfile Profile { get; }
			public string Name => Profile.Name;
		}

		private sealed class MaterialEditor : Form
		{
			private readonly TextBox name = new TextBox { MaxLength = 80, Dock = DockStyle.Fill };
			private readonly ComboBox status = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Dock = DockStyle.Fill };
			private readonly TextBox notes = new TextBox { MaxLength = 600, Multiline = true, ScrollBars = ScrollBars.Vertical, Height = 62, Dock = DockStyle.Fill };
			private readonly OperationEditor engrave;
			private readonly OperationEditor score;
			private readonly OperationEditor cut;

			internal Guid MaterialId { get; }

			internal MaterialEditor(MaterialProfile material, string deviceName, int maxPower, Action<MaterialProfile> saveMaterial)
			{
				MaterialId = material?.Id ?? Guid.NewGuid();
				Text = material == null ? "Uus materjaliprofiil" : "Muuda materjaliprofiili";
				ClientSize = new Size(690, 360);
				MinimumSize = new Size(635, 390);
				StartPosition = FormStartPosition.CenterParent;
				ShowInTaskbar = false;
				AutoScaleMode = AutoScaleMode.Font;

				name.Text = material?.Name ?? string.Empty;
				notes.Text = material?.Notes ?? string.Empty;
				status.Items.AddRange(new object[] { "Tundmatu koostis", "Tuvastatud koostis", "Laseriga töötlemiseks sobimatu" });
				status.SelectedIndex = (int)(material?.Status ?? MaterialStatus.Unverified);

				var layout = new TableLayoutPanel { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(12), ColumnCount = 2, RowCount = 5 };
				layout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 96));
				layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
				layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
				layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
				layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
				layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
				layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
				AddField(layout, 0, "Materjal", name);
				AddField(layout, 1, "Koostis", status);
				AddField(layout, 2, "Märkused ja ohud", notes);

				var operations = new TableLayoutPanel { AutoSize = true, Dock = DockStyle.Fill, ColumnCount = 5, RowCount = 4 };
				operations.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 29));
				operations.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 19));
				operations.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 21));
				operations.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14));
				operations.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 17));
				string[] headings = { "Toiming", "PWM", "Kiirus (mm/min)", "Läbimisi", "Katsetatud" };
				for (int column = 0; column < headings.Length; column++)
					operations.Controls.Add(new Label { Text = headings[column], AutoSize = true }, column, 0);
				engrave = new OperationEditor(operations, 1, "Graveeri", maxPower, material?.Engrave);
				score = new OperationEditor(operations, 2, "Voltimisjoon", maxPower, material?.Score);
				cut = new OperationEditor(operations, 3, "Lõika", maxPower, material?.Cut);
				layout.Controls.Add(operations, 0, 3);
				layout.SetColumnSpan(operations, 2);
				UpdateVerification();
				status.SelectedIndexChanged += (sender, args) => ResetVerification();
				name.TextChanged += (sender, args) => ResetVerification();

				var buttons = new FlowLayoutPanel { AutoSize = true, Dock = DockStyle.Fill, FlowDirection = FlowDirection.RightToLeft, WrapContents = false };
				var cancel = new Button { Text = "Loobu", AutoSize = true, DialogResult = DialogResult.Cancel };
				var save = new Button { Text = "Salvesta", AutoSize = true };
				save.Click += (sender, args) =>
				{
					var profile = new MaterialProfile
					{
						Id = MaterialId,
						DeviceName = deviceName,
						DeviceMaxPower = maxPower,
						Name = name.Text.Trim(),
						Status = (MaterialStatus)status.SelectedIndex,
						Notes = notes.Text.Trim(),
						Engrave = engrave.Read(),
						Score = score.Read(),
						Cut = cut.Read()
					};
					if (TryAction(this, Text, () => saveMaterial(profile)))
						DialogResult = DialogResult.OK;
				};
				buttons.Controls.AddRange(new Control[] { cancel, save });
				layout.Controls.Add(buttons, 0, 4);
				layout.SetColumnSpan(buttons, 2);
				Controls.Add(layout);
				AcceptButton = save;
				CancelButton = cancel;
			}

			private void UpdateVerification()
			{
				bool allowed = status.SelectedIndex == (int)MaterialStatus.Identified && !MaterialProfiles.IsUnsuitableName(name.Text);
				engrave.SetVerificationAllowed(allowed);
				score.SetVerificationAllowed(allowed);
				cut.SetVerificationAllowed(allowed);
			}

			private void ResetVerification()
			{
				engrave.ClearVerification();
				score.ClearVerification();
				cut.ClearVerification();
				UpdateVerification();
			}

			private static void AddField(TableLayoutPanel layout, int row, string title, Control control)
			{
				layout.Controls.Add(new Label { Text = title, AutoSize = true, Anchor = AnchorStyles.Left | AnchorStyles.Top }, 0, row);
				control.AccessibleName = title;
				layout.Controls.Add(control, 1, row);
			}
		}

		private sealed class OperationEditor
		{
			private readonly CheckBox available;
			private readonly CheckBox verified = new CheckBox { AutoSize = true };
			private readonly NumericUpDown power;
			private readonly NumericUpDown feed;
			private readonly NumericUpDown passes;
			private bool verificationAllowed;

			internal OperationEditor(TableLayoutPanel table, int row, string name, int maxPower, MaterialOperation operation)
			{
				available = new CheckBox { Text = name, AutoSize = true, Checked = operation != null };
				verified.AccessibleName = name + " katsetatud";
				verified.Checked = operation?.Confirmed ?? false;
				power = Numeric(name + " PWM", 0, maxPower, operation?.Power ?? 0);
				feed = Numeric(name + " kiirus", 0, 100000, operation?.Feed ?? 0, 3);
				passes = Numeric(name + " läbimisi", 1, 99, operation?.Passes ?? 1);
				table.Controls.Add(available, 0, row);
				table.Controls.Add(power, 1, row);
				table.Controls.Add(feed, 2, row);
				table.Controls.Add(passes, 3, row);
				table.Controls.Add(verified, 4, row);
				available.CheckedChanged += (sender, args) => UpdateEnabled();
				power.ValueChanged += (sender, args) => ClearVerification();
				feed.ValueChanged += (sender, args) => ClearVerification();
				passes.ValueChanged += (sender, args) => ClearVerification();
				UpdateEnabled();
			}

			internal void ClearVerification()
			{
				verified.Checked = false;
			}

			internal void SetVerificationAllowed(bool allowed)
			{
				verificationAllowed = allowed;
				UpdateEnabled();
			}

			internal MaterialOperation Read()
			{
				if (!available.Checked)
					return null;
				return new MaterialOperation { Power = (int)power.Value, Feed = feed.Value, Passes = (int)passes.Value, Confirmed = verified.Enabled && verified.Checked };
			}

			private void UpdateEnabled()
			{
				power.Enabled = feed.Enabled = passes.Enabled = available.Checked;
				verified.Enabled = available.Checked && verificationAllowed;
			}

			private static NumericUpDown Numeric(string name, decimal min, decimal max, decimal value, int decimalPlaces = 0)
			{
				return new NumericUpDown { AccessibleName = name, Minimum = min, Maximum = max, Value = value, DecimalPlaces = decimalPlaces, Dock = DockStyle.Fill };
			}
		}
	}
}
