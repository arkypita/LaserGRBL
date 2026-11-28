using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows.Forms;
using System.Xml;

namespace LaserGRBL.DeviceProfiles
{
	internal sealed class ProfileExportForm : Form
	{
		internal sealed class JobLayer
		{
			internal string Title, Pattern;
			internal string[] Lines;
			internal int Operation, Passes = 1;
			internal bool Overscan, UseMaterial;
		}

		private readonly string folder = Path.Combine(GrblCore.DataPath, "DeviceProfiles");
		private readonly Func<JobLayer> captureCurrent;
		private readonly IList<JobLayer> queue;
		private readonly ComboBox devices = NewCombo();
		private readonly ComboBox patterns = NewCombo();
		private readonly PropertyGrid properties = new PropertyGrid { Dock = DockStyle.Fill, ToolbarVisible = false };
		private readonly NumericUpDown sourceMax = new NumericUpDown { Minimum = 1, Maximum = 100000, Width = 82 };
		private readonly NumericUpDown testPower = new NumericUpDown { Minimum = 1, Maximum = 100, Value = 10, Width = 55 };
		private readonly CheckBox frame = new CheckBox { Text = "Raami seadmes laseriga", AutoSize = true };
		private readonly CheckBox pause = new CheckBox { Text = "Paus enne tööd", AutoSize = true };
		private readonly CheckBox useProfileFeed = new CheckBox { Text = "Kasuta profiili töökiirust", AutoSize = true };
		private readonly DataGridView grid = new DataGridView();
		private readonly Button materialButton = new Button { Text = "Materjal…", AutoSize = true };
		private readonly Label profileState = new Label { AutoSize = true };
		private readonly TabControl tabs = new TabControl { Dock = DockStyle.Fill };
		private DeviceProfile profile;
		private MaterialProfile material;

		internal ProfileExportForm(IList<JobLayer> queue, Func<JobLayer> captureCurrent, int inputPower)
		{
			this.queue = queue;
			this.captureCurrent = captureCurrent;

			Text = "Seadmeprofiilid ja G-koodi eksport";
			StartPosition = FormStartPosition.CenterParent;
			ClientSize = new Size(930, 565);
			MinimumSize = new Size(680, 435);
			AutoScaleMode = AutoScaleMode.Font;
			ShowInTaskbar = false;
			MinimizeBox = false;

			var layout = Panel(2);
			var profileBar = Bar();
			devices.Width = 250;
			profileBar.Controls.Add(devices);
			profileBar.Controls.Add(MakeButton("Uus", () => AddDraft(DeviceProfile.CreateCv01(), "Uus seade")));
			profileBar.Controls.Add(MakeButton("Salvesta profiil", SaveProfile));
			profileBar.Controls.Add(MakeButton("Impordi JSON…", ImportProfile));
			profileBar.Controls.Add(MakeButton("Ekspordi JSON…", ExportProfile));
			layout.Controls.Add(profileBar, 0, 0);

			var work = new TabPage("Töö");
			work.Controls.Add(BuildWorkTab(inputPower));
			var edit = new TabPage("Profiili seaded");
			edit.Controls.Add(properties);
			tabs.TabPages.Add(work);
			tabs.TabPages.Add(edit);
			layout.Controls.Add(tabs, 0, 1);
			Controls.Add(layout);

			devices.SelectedIndexChanged += (sender, args) => SelectProfile();
			properties.PropertyValueChanged += (sender, args) =>
			{
				UpdateProfileControls();
				string property = args.ChangedItem.PropertyDescriptor.Name;
				if (property == "MaxPower" || property == "Name" || property == "LaserOn")
					SetMaterial(null);
				if (property == "FrameByDefault") frame.Checked = frame.Enabled && profile.FrameByDefault;
				if (property == "PauseByDefault") pause.Checked = pause.Enabled && profile.PauseByDefault;
			};
			ReadProfiles();
			RefreshLayers(-1);
		}

		private TableLayoutPanel BuildWorkTab(int inputPower)
		{
			var page = Panel(5);
			page.RowStyles[1].SizeType = SizeType.Percent;
			page.RowStyles[1].Height = 100;
			page.RowStyles[4].SizeType = SizeType.AutoSize;
			var sources = Bar();
			sources.Controls.Add(MakeButton("Lisa avatud töö", AddCurrent));
			sources.Controls.Add(MakeButton("Lisa GRBL-fail…", AddFile));
			sources.Controls.Add(MakeButton("Eemalda valitud", RemoveLayer));
			patterns.Width = 163;
			patterns.Items.AddRange(JobPatterns.Names);
			patterns.SelectedIndex = 0;
			sources.Controls.Add(patterns);
			sources.Controls.Add(MakeButton("Lisa test", AddPattern));
			page.Controls.Add(sources, 0, 0);

			grid.Dock = DockStyle.Fill;
			grid.AllowUserToAddRows = false;
			grid.AllowUserToDeleteRows = false;
			grid.AllowUserToResizeRows = false;
			grid.RowHeadersVisible = false;
			grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			grid.MultiSelect = false;
			grid.EditMode = DataGridViewEditMode.EditOnEnter;
			grid.Columns.Add(new DataGridViewTextBoxColumn
			{
				HeaderText = "Kiht", ReadOnly = true, AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
			});
			var operations = new DataGridViewComboBoxColumn { HeaderText = "Toiming", Width = 102, FlatStyle = FlatStyle.Flat };
			operations.Items.AddRange("Graveerimine", "Voltimisjoon", "Lõikamine");
			grid.Columns.Add(operations);
			grid.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "Läbimisi", Width = 70 });
			grid.Columns.Add(new DataGridViewCheckBoxColumn { HeaderText = "Kiirendusvaru", Width = 100 });
			grid.Columns.Add(new DataGridViewCheckBoxColumn { HeaderText = "Kasuta materjali", Width = 110 });
			foreach (DataGridViewColumn column in grid.Columns)
				column.SortMode = DataGridViewColumnSortMode.NotSortable;
			page.Controls.Add(grid, 0, 1);

			var options = Bar();
			sourceMax.Value = Math.Max(1, Math.Min(100000, inputPower));
			options.Controls.Add(MakeLabel("Algtöö max. S"));
			options.Controls.Add(sourceMax);
			options.Controls.Add(MakeLabel("Testi võimsus (%)"));
			options.Controls.Add(testPower);
			materialButton.Click += (sender, args) => Perform(ChooseMaterial);
			options.Controls.Add(materialButton);
			options.Controls.Add(frame);
			options.Controls.Add(pause);
			options.Controls.Add(useProfileFeed);
			page.Controls.Add(options, 0, 2);

			page.Controls.Add(profileState, 0, 3);

			var buttons = Bar();
			buttons.Controls.Add(MakeButton("Eelvaade ekraanil", Preview));
			buttons.Controls.Add(MakeButton("Salvesta G-kood…", SaveJob));
			buttons.Controls.Add(MakeButton("Tühjenda ja salvesta KAART", SaveCard));
			var close = new Button { Text = "Sulge", AutoSize = true, DialogResult = DialogResult.Cancel };
			buttons.Controls.Add(close);
			CancelButton = close;
			page.Controls.Add(buttons, 0, 4);
			FormClosing += (sender, args) => { grid.EndEdit(); SaveLayerOptions(false); };
			return page;
		}

		private void ReadProfiles()
		{
			devices.Items.Add(new ProfileItem(DeviceProfile.CreateCv01(), null));
			if (Directory.Exists(folder))
			{
				foreach (string path in Directory.GetFiles(folder, "device-*.json"))
				{
					try { devices.Items.Add(new ProfileItem(DeviceProfile.Read(path), path)); }
					catch (Exception ex)
					{
						if (!(ex is IOException || ex is InvalidDataException || ex is SerializationException || ex is XmlException || ex is UnauthorizedAccessException))
							throw;
						MessageBox.Show(this, Path.GetFileName(path) + "\n" + ex.Message, Text,
							MessageBoxButtons.OK, MessageBoxIcon.Warning);
					}
				}
			}
			devices.SelectedIndex = 0;
		}

		private void SelectProfile()
		{
			var selected = (ProfileItem)devices.SelectedItem;
			if (selected == null)
				return;
			profile = selected.Profile.Copy();
			properties.SelectedObject = profile;
			SetMaterial(null);
			UpdateProfileControls();
			frame.Checked = frame.Enabled && profile.FrameByDefault;
			pause.Checked = pause.Enabled && profile.PauseByDefault;
		}

		private void UpdateProfileControls()
		{
			frame.Enabled = profile.PreviewPasses > 0 && profile.PreviewPower > 0;
			pause.Enabled = !string.IsNullOrWhiteSpace(profile.PauseCommand);
			if (!frame.Enabled) frame.Checked = false;
			if (!pause.Enabled) pause.Checked = false;
			profileState.Text = string.Format(CultureInfo.CurrentCulture,
				"{0:0.##} × {1:0.##} mm  ·  Võimsus {2}–{3}  ·  Töökiirus {4:0.##} mm/min  ·  ASCII {5}",
				profile.Width, profile.Height, profile.MinPower, profile.MaxPower, profile.WorkFeed, profile.Extension);
		}

		private void AddDraft(DeviceProfile initial, string name)
		{
			initial.Name = name;
			int index = devices.Items.Add(new ProfileItem(initial, null));
			devices.SelectedIndex = index;
			tabs.SelectedIndex = 1;
			properties.Focus();
		}

		private void SaveProfile()
		{
			profile.Validate();
			var selected = (ProfileItem)devices.SelectedItem;
			if (selected.Path == null && string.Equals(profile.Name, DeviceProfile.CreateCv01().Name, StringComparison.OrdinalIgnoreCase))
				throw new InvalidDataException("Sisseehitatud profiili koopia salvestamiseks anna sellele uus nimi.");
			foreach (ProfileItem other in devices.Items)
			{
				if (other != selected && string.Equals(other.Profile.Name, profile.Name, StringComparison.OrdinalIgnoreCase))
					throw new InvalidDataException("Selle nimega profiil on juba olemas.");
			}
			Directory.CreateDirectory(folder);
			string path = selected.Path ?? Path.Combine(folder, "device-" + Guid.NewGuid().ToString("N") + ".json");
			profile.Write(path);
			int index = devices.SelectedIndex;
			devices.Items[index] = new ProfileItem(profile.Copy(), path);
			devices.SelectedIndex = index;
			profileState.Text += "  ·  Salvestatud";
		}

		private void ImportProfile()
		{
			using (var dialog = new OpenFileDialog { Filter = "Seadmeprofiil (*.json)|*.json", RestoreDirectory = true })
			{
				if (dialog.ShowDialog(this) == DialogResult.OK)
				{
					DeviceProfile imported = DeviceProfile.Read(dialog.FileName);
					string name = imported.Name;
					for (int suffix = 2; devices.Items.Cast<ProfileItem>().Any(item =>
						string.Equals(item.Profile.Name, name, StringComparison.OrdinalIgnoreCase)); suffix++)
						name = imported.Name.Substring(0, Math.Min(imported.Name.Length, 85)) + " (" + suffix + ")";
					AddDraft(imported, name);
				}
			}
		}

		private void ExportProfile()
		{
			profile.Validate();
			using (var dialog = SaveDialog(".json", "Seadmeprofiil"))
				if (dialog.ShowDialog(this) == DialogResult.OK)
					profile.Write(dialog.FileName);
		}

		private void AddCurrent()
		{
			JobLayer layer = captureCurrent();
			if (layer == null)
				throw new InvalidDataException("Ava esmalt töö LaserGRBL-is ja oota teisenduse lõpuni.");
			AddLayer(layer);
		}

		private void AddFile()
		{
			using (var dialog = new OpenFileDialog
			{
				Filter = "GRBL G-kood|*.nc;*.gcode;*.gc;*.tap;*.ngc;*.txt|Kõik failid|*.*",
				RestoreDirectory = true
			})
			{
				if (dialog.ShowDialog(this) != DialogResult.OK)
					return;
				if (new FileInfo(dialog.FileName).Length > 32000000)
					throw new InvalidDataException("G-koodi fail on liiga suur.");
				AddLayer(new JobLayer { Title = Path.GetFileName(dialog.FileName), Lines = File.ReadAllLines(dialog.FileName) });
			}
		}

		private void AddPattern()
		{
			string name = (string)patterns.SelectedItem;
			AddLayer(new JobLayer { Title = name, Pattern = name });
		}

		private void AddLayer(JobLayer layer)
		{
			SaveLayerOptions(true);
			queue.Add(layer);
			RefreshLayers(queue.Count - 1);
		}

		private void RemoveLayer()
		{
			if (grid.CurrentRow == null)
				return;
			int index = grid.CurrentRow.Index;
			SaveLayerOptions(false);
			queue.RemoveAt(index);
			RefreshLayers(Math.Min(index, queue.Count - 1));
		}

		private void RefreshLayers(int selection)
		{
			var operations = (DataGridViewComboBoxColumn)grid.Columns[1];
			grid.Rows.Clear();
			foreach (JobLayer layer in queue)
				grid.Rows.Add(layer.Title, operations.Items[layer.Operation], layer.Passes, layer.Overscan, layer.UseMaterial);
			if (selection >= 0)
				grid.CurrentCell = grid.Rows[selection].Cells[0];
		}

		private void SaveLayerOptions(bool strict)
		{
			grid.EndEdit();
			foreach (DataGridViewRow row in grid.Rows)
			{
				JobLayer layer = queue[row.Index];
				int operation = ((DataGridViewComboBoxColumn)grid.Columns[1]).Items.IndexOf(row.Cells[1].Value);
				if (operation >= 0)
					layer.Operation = operation;
				layer.Overscan = true.Equals(row.Cells[3].Value);
				layer.UseMaterial = true.Equals(row.Cells[4].Value);
				int passes;
				if (int.TryParse(Convert.ToString(row.Cells[2].Value, CultureInfo.CurrentCulture), out passes) &&
					passes >= 1 && passes <= 100)
					layer.Passes = passes;
				else if (strict && !layer.UseMaterial)
					throw new InvalidDataException("Läbimiste arv peab olema 1–100.");
			}
		}

		private void ChooseMaterial()
		{
			profile.Validate();
			string deviceName = profile.Name == "Creality CV-01" && profile.LaserOn != "M106 S{power}" ? "" : profile.Name;
			MaterialProfile choice = MaterialProfileForm.Choose(this, folder, profile.MaxPower, deviceName);
			if (choice != null)
				SetMaterial(choice);
		}

		private void SetMaterial(MaterialProfile choice)
		{
			material = choice;
			materialButton.Text = choice == null ? "Materjal…" : choice.Name + "…";
		}

		private ExportedJob BuildJob(bool confirm)
		{
			profile.Validate();
			SaveLayerOptions(true);
			var warnings = new HashSet<string>();
			var layers = new List<ExportLayer>();
			int inputMax = (int)sourceMax.Value;
			foreach (JobLayer layer in queue.OrderBy(item => item.Operation))
			{
				var kind = (MaterialOperationKind)layer.Operation;
				MaterialOperation preset = layer.UseMaterial && material != null ? material.GetOperation(kind) : null;
				if (layer.UseMaterial && (preset == null || material.IsUnsuitable))
					throw new InvalidDataException("Vali kihile sobiv materjal ja määra selle toimingu seaded.");
				if (layer.Pattern != null && layer.UseMaterial)
					throw new InvalidDataException("Testkiht kasutab testi võimsust ja profiili töökiirust.");
				string warning = MaterialProfileForm.GetWarning(material, kind);
				if (!string.IsNullOrEmpty(warning))
					warnings.Add(warning);
				if (material != null && layer.Pattern == null && !layer.UseMaterial)
					warnings.Add("Kihil „" + layer.Title + "” on käsitsi seadistatud tööparameetrid.");
				decimal width = profile.ReverseX ? profile.OriginX : profile.Width - profile.OriginX;
				decimal height = profile.ReverseY ? profile.OriginY : profile.Height - profile.OriginY;
				layers.Add(new ExportLayer
				{
					Lines = layer.Pattern == null ? layer.Lines : JobPatterns.Generate(layer.Pattern, width, height, inputMax, profile.WorkFeed),
					Passes = preset == null ? layer.Passes : preset.Passes,
					OperationPower = layer.Pattern != null ? (int)Math.Max(1, Math.Floor(profile.MaxPower * testPower.Value / 100)) : preset?.Power,
					WorkFeed = preset?.Feed ?? (layer.Pattern == null && useProfileFeed.Checked ? profile.WorkFeed : (decimal?)null),
					ApplyOverscan = layer.Pattern == null && layer.Overscan
				});
			}
			ExportedJob job = ProfileExporter.Create(profile, layers, inputMax, frame.Checked, pause.Checked);
			if (!confirm)
				return job;
			if (material != null && !string.IsNullOrEmpty(material.Notes))
				warnings.Add(material.Notes);
			if (frame.Checked)
				warnings.Add("Fail alustab seadmes madala võimsusega laserraamimisega.");
			if (profile.LaserOn.StartsWith("M106 ", StringComparison.Ordinal))
				warnings.Add("Ventilaatoriväljundi PWM-i ajastus vajab kinnitamist seadme püsivaral.");
			if (warnings.Count != 0 && MessageBox.Show(this, string.Join("\n\n", warnings) + "\n\nKas salvestada G-kood?",
				"Kontrolli töö seadistust", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
				return null;
			return job;
		}

		private void Preview()
		{
			ExportedJob job = BuildJob(false);
			JobPreview.Show(this, profile, job.Moves);
		}

		private void SaveJob()
		{
			ExportedJob job = BuildJob(true);
			if (job == null) return;
			using (var dialog = SaveDialog(profile.Extension, "G-kood"))
				if (dialog.ShowDialog(this) == DialogResult.OK)
					ProfileFile.Write(dialog.FileName, job.Bytes);
		}

		private void SaveCard()
		{
			ExportedJob job = BuildJob(true);
			if (job != null)
				CardDelivery.ShowDialog(this, job.Bytes, profile.Extension);
		}

		private void Perform(Action action)
		{
			try { action(); }
			catch (Exception ex) when (ex is IOException || ex is InvalidDataException || ex is ArgumentException || ex is InvalidOperationException ||
				ex is UnauthorizedAccessException || ex is SerializationException || ex is XmlException || ex is OverflowException)
			{
				MessageBox.Show(this, ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
			}
		}

		private Button MakeButton(string text, Action action)
		{
			var button = new Button { Text = text, AutoSize = true };
			button.Click += (sender, args) => Perform(action);
			return button;
		}

		private static Label MakeLabel(string text) { return new Label { Text = text, AutoSize = true, Margin = new Padding(6, 8, 1, 0) }; }

		private static TableLayoutPanel Panel(int rows)
		{
			var panel = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(8), RowCount = rows, ColumnCount = 1 };
			panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
			for (int i = 0; i < rows - 1; i++)
				panel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
			panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
			return panel;
		}

		private static FlowLayoutPanel Bar() { return new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true }; }

		private static ComboBox NewCombo() { return new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList }; }

		private static SaveFileDialog SaveDialog(string extension, string label)
		{
			return new SaveFileDialog { Filter = label + "|*" + extension, DefaultExt = extension.TrimStart('.'),
				AddExtension = true, RestoreDirectory = true };
		}

		private sealed class ProfileItem
		{
			internal readonly DeviceProfile Profile;
			internal readonly string Path;

			internal ProfileItem(DeviceProfile profile, string path)
			{
				Profile = profile;
				Path = path;
			}

			public override string ToString() { return Profile.Name; }
		}
	}
}
