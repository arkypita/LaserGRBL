//Copyright (c) 2016-2021 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using LaserGRBL.RasterConverter;
using LaserGRBL.UserControls;
using LaserGRBL.UserControls.NumericInput;
using ComboboxItem = LaserGRBL.SvgConverter.SvgToGCodeForm.ComboboxItem;

namespace LaserGRBL.SvgConverter
{
	/// <summary>
	/// Vertical accordion with one item for each color layer: the header shows the color and its mode,
	/// the open item (only one at a time) contains the layer settings
	/// </summary>
	public class SvgLayerAccordion : Panel
	{
		public event Action<SvgColorLayer> LayerClicked; // header clicked: the layer to select, null to close it
		public event Action<SvgColorLayer> LayerChanged; // a setting of the layer was edited

		private List<Item> mItems = new List<Item>();
		private SvgColorLayer mSelected;

		public SvgLayerAccordion()
		{
			AutoScroll = true;
			BackColor = ColorScheme.FormBackColor;
			ForeColor = ColorScheme.FormForeColor;
		}

		public void SetLayers(List<SvgColorLayer> layers, int maxSpeed, int maxPower)
		{
			SuspendLayout();
			foreach (Item item in mItems)
				item.Dispose();
			mItems.Clear();

			foreach (SvgColorLayer layer in layers)
				mItems.Add(new Item(this, layer, maxSpeed, maxPower));

			// docked controls are stacked from the last one: add them in reverse order to get the first at the top
			for (int i = mItems.Count - 1; i >= 0; i--)
				Controls.Add(mItems[i]);
			ResumeLayout(true);
		}

		public SvgColorLayer SelectedLayer
		{
			get { return mSelected; }
			set
			{
				mSelected = value;
				SuspendLayout();
				foreach (Item item in mItems)
					item.Expanded = item.Layer == value;
				ResumeLayout(true);

				Item selected = mItems.Find(i => i.Layer == value);
				if (selected != null)
					ScrollControlIntoView(selected);
			}
		}

		/// <summary>
		/// Show again the values of the layers, changed outside the accordion
		/// </summary>
		public void RefreshValues()
		{
			foreach (Item item in mItems)
				item.RefreshValues();
		}

		private void OnHeaderClick(Item item)
		{
			if (LayerClicked != null)
				LayerClicked(item.Expanded ? null : item.Layer);
		}

		private void OnValueChanged(Item item)
		{
			if (LayerChanged != null)
				LayerChanged(item.Layer);
		}

		/// <summary>
		/// One color: header always visible, settings visible when expanded
		/// </summary>
		private class Item : TableLayoutPanel
		{
			public readonly SvgColorLayer Layer;
			private SvgLayerAccordion mOwner;
			private Header mHeader;
			private TableLayoutPanel mBody;
			private FlatComboBox mMode, mPattern;
			private IntegerInputRanged mSpeed, mPower, mPasses;
			private DecimalInputRanged mLinesPerMM;
			private Control[] mFillRow;
			private bool mUpdating;

			public Item(SvgLayerAccordion owner, SvgColorLayer layer, int maxSpeed, int maxPower)
			{
				mOwner = owner;
				Layer = layer;

				Dock = DockStyle.Top;
				AutoSize = true;
				AutoSizeMode = AutoSizeMode.GrowAndShrink;
				ColumnCount = 1;
				ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
				Margin = Padding.Empty;
				Padding = Padding.Empty;

				mHeader = new Header(this);
				Controls.Add(mHeader, 0, 0);

				mBody = new TableLayoutPanel();
				mBody.AutoSize = true;
				mBody.AutoSizeMode = AutoSizeMode.GrowAndShrink;
				mBody.Dock = DockStyle.Fill;
				mBody.ColumnCount = 3;
				mBody.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
				mBody.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
				mBody.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
				mBody.Padding = new Padding(28, 2, 6, 8);
				mBody.Margin = Padding.Empty;
				mBody.Visible = false;

				mMode = CreateCombo(new ComboboxItem[] {
					new ComboboxItem(Strings.SvgLayerModeLine, SvgLayerMode.Line),
					new ComboboxItem(Strings.SvgLayerModeFill, SvgLayerMode.Fill),
					new ComboboxItem(Strings.SvgLayerModeFillLine, SvgLayerMode.FillAndLine),
					new ComboboxItem(Strings.SvgLayerModeCut, SvgLayerMode.Cut),
					new ComboboxItem(Strings.SvgLayerModeIgnore, SvgLayerMode.Ignore) });
				mSpeed = CreateInteger(1, maxSpeed);
				mPower = CreateInteger(0, maxPower);
				mPasses = CreateInteger(1, 100);

				List<ComboboxItem> patterns = new List<ComboboxItem>();
				foreach (ImageProcessor.Direction direction in Enum.GetValues(typeof(ImageProcessor.Direction)))
					if (GrblFile.VectorFilling(direction))
						patterns.Add(new ComboboxItem(GrblCore.TranslateEnum(direction), direction));
				mPattern = CreateCombo(patterns.ToArray());

				mLinesPerMM = new DecimalInputRanged();
				mLinesPerMM.MinValue = 0.5f;
				mLinesPerMM.MaxValue = 50;
				mLinesPerMM.DecimalPositions = 2;
				mLinesPerMM.Anchor = AnchorStyles.Left | AnchorStyles.Right;

				AddRow(Strings.SvgLayerMode, mMode, null);
				AddRow(Strings.SvgLayerSpeed, mSpeed, "mm/min");
				AddRow(Strings.SvgLayerPower, mPower, "S");
				AddRow(Strings.SvgLayerPasses, mPasses, null);
				Control[] pattern = AddRow(Strings.SvgLayerFillPattern, mPattern, null);
				Control[] lines = AddRow(Strings.SvgLayerLinesPerMM, mLinesPerMM, null);
				mFillRow = new Control[] { pattern[0], pattern[1], lines[0], lines[1] };

				Controls.Add(mBody, 0, 1);
				ThemeMgr.SetTheme(mBody, true);

				RefreshValues();

				mMode.SelectedIndexChanged += delegate { if (!mUpdating) { Layer.Mode = (SvgLayerMode)((ComboboxItem)mMode.SelectedItem).Value; Changed(); } };
				mPattern.SelectedIndexChanged += delegate { if (!mUpdating) { Layer.FillDirection = (ImageProcessor.Direction)((ComboboxItem)mPattern.SelectedItem).Value; Changed(); } };
				mSpeed.CurrentValueChanged += delegate (object s, int o, int n, bool u) { if (!mUpdating) { Layer.Speed = Clamp(mSpeed, n); Changed(); } };
				mPower.CurrentValueChanged += delegate (object s, int o, int n, bool u) { if (!mUpdating) { Layer.Power = Clamp(mPower, n); Changed(); } };
				mPasses.CurrentValueChanged += delegate (object s, int o, int n, bool u) { if (!mUpdating) { Layer.Passes = Clamp(mPasses, n); Changed(); } };
				mLinesPerMM.CurrentValueChanged += delegate (object s, float o, float n, bool u) { if (!mUpdating) { Layer.LinesPerMM = Math.Max(0.5, Math.Min(50, Math.Round(n, 2))); Changed(); } };
			}

			private static FlatComboBox CreateCombo(ComboboxItem[] items)
			{
				FlatComboBox cb = new FlatComboBox();
				cb.DropDownStyle = ComboBoxStyle.DropDownList;
				cb.Items.AddRange(items);
				cb.Anchor = AnchorStyles.Left | AnchorStyles.Right;
				return cb;
			}

			private static IntegerInputRanged CreateInteger(int min, int max)
			{
				IntegerInputRanged ii = new IntegerInputRanged();
				ii.MinValue = min;
				ii.MaxValue = max;
				ii.Anchor = AnchorStyles.Left | AnchorStyles.Right;
				return ii;
			}

			// out of range values are brought back in range, as the old table did
			private int Clamp(IntegerInputRanged ii, int value)
			{
				int clamped = Math.Max(ii.MinValue, Math.Min(ii.MaxValue, value));
				if (clamped != value)
				{
					mUpdating = true;
					ii.CurrentValue = clamped;
					mUpdating = false;
				}
				return clamped;
			}

			private Control[] AddRow(string caption, Control input, string unit)
			{
				int row = mBody.RowCount++;
				mBody.RowStyles.Add(new RowStyle(SizeType.AutoSize));

				Label label = new Label();
				label.Text = caption;
				label.AutoSize = true;
				label.Anchor = AnchorStyles.Left;
				mBody.Controls.Add(label, 0, row);
				mBody.Controls.Add(input, 1, row);

				if (unit != null)
				{
					Label u = new Label();
					u.Text = unit;
					u.AutoSize = true;
					u.Anchor = AnchorStyles.Left;
					mBody.Controls.Add(u, 2, row);
				}
				return new Control[] { label, input };
			}

			public void RefreshValues()
			{
				mUpdating = true;
				SelectValue(mMode, Layer.Mode);
				SelectValue(mPattern, Layer.FillDirection);
				mSpeed.CurrentValue = Layer.Speed;
				mPower.CurrentValue = Layer.Power;
				mPasses.CurrentValue = Layer.Passes;
				mLinesPerMM.CurrentValue = (float)Layer.LinesPerMM;
				mUpdating = false;
				RefreshFillRow();
				mHeader.Invalidate();
			}

			private static void SelectValue(ComboBox cb, object value)
			{
				foreach (ComboboxItem item in cb.Items)
					if (item.Value.Equals(value))
						cb.SelectedItem = item;
			}

			// fill pattern and line density are meaningful only for fill modes
			private void RefreshFillRow()
			{
				foreach (Control c in mFillRow)
					c.Visible = Layer.HasFill;
			}

			private void Changed()
			{
				RefreshFillRow();
				mHeader.Invalidate();
				mOwner.OnValueChanged(this);
			}

			public bool Expanded
			{
				get { return mBody.Visible; }
				set
				{
					if (mBody.Visible != value)
					{
						mBody.Visible = value;
						mHeader.Invalidate();
					}
				}
			}

			public void HeaderClicked()
			{ mOwner.OnHeaderClick(this); }
		}

		/// <summary>
		/// Clickable header of an item: swatch, color code, mode, number of elements and expand arrow
		/// </summary>
		private class Header : Control
		{
			private Item mItem;
			private bool mHover;
			private ToolTip mToolTip = new ToolTip();

			public Header(Item item)
			{
				mItem = item;
				SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
				Dock = DockStyle.Fill;
				Height = 30;
				Margin = Padding.Empty;
				Cursor = Cursors.Hand;
				mToolTip.SetToolTip(this, item.Layer.Color + " - " + string.Format(Strings.SvgLayerElements, item.Layer.ElementCount));
			}

			protected override void OnClick(EventArgs e)
			{
				base.OnClick(e);
				mItem.HeaderClicked();
			}

			protected override void OnMouseEnter(EventArgs e)
			{ base.OnMouseEnter(e); mHover = true; Invalidate(); }

			protected override void OnMouseLeave(EventArgs e)
			{ base.OnMouseLeave(e); mHover = false; Invalidate(); }

			protected override void OnPaint(PaintEventArgs e)
			{
				Graphics g = e.Graphics;
				SvgColorLayer layer = mItem.Layer;
				bool ignored = layer.Mode == SvgLayerMode.Ignore;
				Color back = mItem.Expanded ? ColorScheme.FormButtonsColor : mHover ? Blend(ColorScheme.FormBackColor, ColorScheme.FormButtonsColor, 0.5) : ColorScheme.FormBackColor;
				Color fore = ignored ? SystemColors.GrayText : ColorScheme.FormForeColor;
				g.Clear(back);

				// color swatch
				Rectangle swatch = new Rectangle(8, (Height - 16) / 2, 16, 16);
				using (Brush b = new SolidBrush(layer.DrawingColor))
					g.FillRectangle(b, swatch);
				using (Pen p = new Pen(Color.FromArgb(128, ColorScheme.FormForeColor)))
					g.DrawRectangle(p, swatch);

				// expand arrow on the right
				int ax = Width - 16, ay = Height / 2;
				Point[] arrow = mItem.Expanded
					? new Point[] { new Point(ax - 4, ay - 2), new Point(ax + 4, ay - 2), new Point(ax, ay + 3) }
					: new Point[] { new Point(ax - 2, ay - 4), new Point(ax + 3, ay), new Point(ax - 2, ay + 4) };
				g.SmoothingMode = SmoothingMode.AntiAlias;
				using (Brush b = new SolidBrush(fore))
					g.FillPolygon(b, arrow);
				g.SmoothingMode = SmoothingMode.None;

				// "mode · count" right aligned, color code on the left
				string right = ModeText(layer.Mode) + " · " + layer.ElementCount;
				TextFormatFlags flags = TextFormatFlags.VerticalCenter | TextFormatFlags.SingleLine | TextFormatFlags.NoPadding;
				Rectangle text = new Rectangle(32, 0, Width - 32 - 28, Height);
				TextRenderer.DrawText(g, right, Font, text, fore, flags | TextFormatFlags.Right);
				using (Font bold = new Font(Font, mItem.Expanded ? FontStyle.Bold : FontStyle.Regular))
					TextRenderer.DrawText(g, layer.Color, bold, text, fore, flags | TextFormatFlags.Left | TextFormatFlags.EndEllipsis);

				using (Pen p = new Pen(Color.FromArgb(40, ColorScheme.FormForeColor)))
					g.DrawLine(p, 0, Height - 1, Width, Height - 1);
			}

			private static string ModeText(SvgLayerMode mode)
			{
				switch (mode)
				{
					case SvgLayerMode.Fill: return Strings.SvgLayerModeFill;
					case SvgLayerMode.FillAndLine: return Strings.SvgLayerModeFillLine;
					case SvgLayerMode.Cut: return Strings.SvgLayerModeCut;
					case SvgLayerMode.Ignore: return Strings.SvgLayerModeIgnore;
					default: return Strings.SvgLayerModeLine;
				}
			}

			private static Color Blend(Color a, Color b, double amount)
			{
				return Color.FromArgb((int)(a.R + (b.R - a.R) * amount), (int)(a.G + (b.G - a.G) * amount), (int)(a.B + (b.B - a.B) * amount));
			}

			protected override void Dispose(bool disposing)
			{
				if (disposing)
					mToolTip.Dispose();
				base.Dispose(disposing);
			}
		}
	}
}
