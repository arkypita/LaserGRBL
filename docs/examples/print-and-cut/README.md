# Print and Cut: example files

Two small files to try [Print and Cut](../../PrintAndCut.md) without preparing your own design. Both were
created for this repository (no third-party artwork) and can be used freely.

| File | Use it to |
|---|---|
| `print-and-cut-test-print.svg` | **Print** it. A 120 x 70 mm test pattern (10 mm grid, 5 mm sub-grid, diagonals), the dashed outline of the cut and the two registration marks in black. |
| `print-and-cut-test-cut.svg` | **Load** it in LaserGRBL. The cut (a rounded rectangle, two holes and a slot) plus the two registration marks in `#FF00FF`. |

Page size is 160 x 110 mm. The two marks are circles with a cross (center to center: 160.9 mm), at opposite
corners of the page, drawn with 0.2 mm strokes.

## How to try it

1. Print `print-and-cut-test-print.svg` at **100%** (no "fit to page"). Open it in Inkscape, a browser or any
   viewer that respects the physical size. Check with a ruler that the distance between the centers of the two
   printed crosses is 160.9 mm.
2. Put the sheet on the bed, crooked or shifted: that is the point. Fix it with tape.
3. In LaserGRBL, open `print-and-cut-test-cut.svg` (*File > Open*). The wizard (`Alt+0`) should say
   *2 registration marks found*. Mark **#1** is the top-left one and **#2** the bottom-right one.
4. Set the first and the second target on the centers of the printed crosses, then choose *No Scaling*
   (or *Scaled* if the print is not exactly 160.9 mm). See the [step by step guide](../../PrintAndCut.md).
5. The 3D preview moves the cut onto the sheet. Do a framing and then cut with a low power or a high speed:
   the outline should follow the dashed line printed on the sheet, and the holes should fall on the grid
   intersections at the same positions as in the cut file (for example the first hole is centered 30 mm
   from the left edge of the pattern and 35 mm from its top edge).

The printed marks may be of any color (they are black here): LaserGRBL only reads the marks of the cut file.
