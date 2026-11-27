# Print and Cut

Print and Cut lets you cut or engrave on a material that already has a printed design, even if the sheet
is placed crooked or shifted on the bed. You tell LaserGRBL where **two registration marks** of the printed
sheet really are; LaserGRBL then rotates, moves (and optionally scales) the whole job so that the cut falls
exactly on the print.

It mirrors LightBurn's *Print and Cut* feature: same menu names, same workflow and same shortcuts, so
LightBurn tutorials are a good complement to this page.

> **Status:** the alignment math, the G-code rewriting and the automated tests are covered by unit tests.
> The wizard, the shortcuts and the aiming laser have been exercised on a real diode laser, but please report
> anything that does not behave as described here.

## Contents

1. [Concepts](#1-concepts)
2. [Prepare the design (SVG)](#2-prepare-the-design-svg)
3. [Print](#3-print)
4. [Prepare the machine](#4-prepare-the-machine)
5. [Import the design](#5-import-the-design)
6. [Open the wizard](#6-open-the-wizard)
7. [Set the first target](#7-set-the-first-target)
8. [Set the second target](#8-set-the-second-target)
9. [Align (with or without scaling)](#9-align-with-or-without-scaling)
10. [Verify before cutting](#10-verify-before-cutting)
11. [Run or save the job](#11-run-or-save-the-job)
12. [Correct, repeat and reset](#12-correct-repeat-and-reset)
13. [Shortcuts and menu](#13-shortcuts-and-menu)
14. [Laser safety](#14-laser-safety)
15. [Accuracy tips](#15-accuracy-tips)
16. [Testing without a machine](#16-testing-without-a-machine)
17. [Messages and what to do](#17-messages-and-what-to-do)
18. [Known limitations](#18-known-limitations)
19. [Quick checklist](#19-quick-checklist)

Example files to try it without preparing a design: [`docs/examples/print-and-cut`](examples/print-and-cut/README.md).

---

## 1. Concepts

**Registration mark.** A small shape that you draw in your SVG with a reserved color. It is only a
reference: it is never cut or engraved.

**Reserved color.** Magenta `#FF00FF` (red 255, green 0, blue 255). Colors very close to it are accepted
(up to 24 levels of difference per channel). Anything in the SVG with that color, as stroke **or** fill, is a
mark. The color cannot be changed from the user interface.

**Two-mark rule.** Magenta is only treated as a mark (and therefore *not* cut or engraved) when the job has at
least **two** marks. An SVG with a single magenta element is cut or engraved like any other color and no mark
appears.

Only colors written as `#RRGGBB` or by name are recognized; `rgb()`, gradients and `currentColor` are not.

**Mark center.** The center of the mark's bounding box. Use symmetric shapes (a circle, a cross, a dot): for an
asymmetric shape the computed center is not the center you see.

**Target.** A mark that you have associated with a real head position. There are two: the first and the
second target.

**Align.** With both targets set, LaserGRBL computes rotation, translation and (optionally) scale, and applies
them to the **whole loaded job**.

**Mark colors in the preview** (as in LightBurn):

| Color | Meaning |
|---|---|
| Magenta | Unused mark |
| Red | Mark chosen as first target (already set) |
| Blue | Mark chosen as second target (already set) |
| Green | Targets, with the alignment enabled |

Each mark is labeled `#1`, `#2`, ... and, when it is a target, `#1 (1)` or `#2 (2)`.

**Mark numbering.** Marks are numbered in reading order: top to bottom and, within a row, left to right.
The numbering has nothing to do with the order in which you drew them, and it does not matter which mark you
use as first or second target, as long as you point the head at the mark you selected.

## 2. Prepare the design (SVG)

Do this in your drawing program (Inkscape, Illustrator...).

1. Draw your design as usual, with the colors you want for cutting or engraving. **Do not use pure magenta
   in the design itself**: everything magenta is taken as a mark.
2. Draw **at least two marks** with color `#FF00FF`.
   - Any shape works; a small, symmetric one is best. A circle with a cross inside is a good choice: the
     center of the cross is where you will point the laser.
   - Stroke or fill, both work.
   - Recommended size: 3 to 10 mm. Use thin lines (0.2 to 0.3 mm) for crosses.
   - A cross made of two lines counts as **one** mark.
3. Place them **as far apart as possible**, for example at opposite corners of the design. The farther apart,
   the smaller the rotation error caused by a small pointing error. The absolute minimum between the two
   centers is 1 mm; closer than that the alignment is refused.
4. Grouping rules:
   - Magenta strokes less than 1 mm apart are grouped into one mark.
   - Two different marks must be more than 1 mm apart.
   - If a resulting mark is larger than 20 mm, you get a warning: several marks were probably merged, or
     there is other magenta in the drawing.
5. Save as SVG, using millimeters. The drawing size in the SVG must be the real size you want.

In Inkscape: select the mark, open *Fill and Stroke* (`Ctrl+Shift+F`), choose *Stroke paint* (or *Fill*),
switch the color model to **RGB** and enter `R=255, G=0, B=255, A=255` (RGBA `ff00ffff`).

## 3. Print

1. Print the design (with the marks visible) at **100%**. Do not use "fit to page" or "shrink to fit".
2. Keep a white margin so you can hold the sheet.
3. Optional but recommended: measure the distance between the two printed marks with a ruler and compare it
   with the design. If they differ, you will use *Scaled* in step 9; if they match, use *No Scaling*.

The printed marks can be any color (even black), as long as you can see their center: LaserGRBL never looks
at the print. Only the SVG you load needs the magenta marks, in the same positions as the printed ones.

## 4. Prepare the machine

1. Put the sheet on the bed. It can be crooked or anywhere: that is exactly what this feature is for.
2. Fix it firmly (tape). If it moves after the targets are set, repeat the process.
3. Focus the laser at the material height, as usual.
4. Connect the machine. The status must change from *Disconnected* to *Idle*. Without a connection, the
   *Set*, *Jog* and *Laser* buttons are disabled.
5. If the controller is in alarm, unlock it. If your machine has homing, do it now (before placing the sheet,
   so the head does not drag over the material).
6. After homing you can move the head freely; only the position at the moment you press *Set* matters.
7. Do **not** reset the board, power-cycle the machine or home again between setting the targets and running
   the job. Targets are stored as machine coordinates; if the machine zero changes, set them again.
   Connecting or disconnecting also clears the targets, for the same reason.
   Changing the work zero (X0 Y0) after setting the targets *is* safe: it is taken into account.

## 5. Import the design

**Case A: only the SVG**

1. *File → Open file*, choose the SVG.
2. In the SVG import dialog choose your usual options (size, speed, power, color filter) and accept.
3. You will see your design and the marks in magenta with their number. The marks are **not** in the G-code
   that is cut.

**Case B: an SVG with marks plus an image (PNG/JPG) to engrave**

1. **First** open the SVG with the marks (*File → Open file*). If you open the image first, or open another
   file with the normal *Open* afterwards, the marks are lost.
2. **Then** add the image with *Append* (not *Open file*). This combines it with the SVG job and keeps the marks.
3. Marks are read only from the SVG. The image must be placed in the same coordinates as the SVG (same origin
   and consistent sizes in millimeters).
4. The alignment applies to the whole job (SVG + image). There is no mark detection inside images.

## 6. Open the wizard

*Tools → Print and Cut → Start Wizard* (or `Alt+0`). The *Print and Cut Wizard* window opens at the top right
of the main window and can stay open while you use the rest of the program.

The message at the top tells you the state:

- `N registration marks found (color #FF00FF)...`: all good.
- `No registration marks found...`: the SVG has no magenta marks, or it was loaded wrongly. A single mark is
  not enough: at least two are needed.
- `Mark #N is X x Y mm: several marks may have been merged...`: a mark is very large.

The window has a block for the **first** target and a block for the **second** target (each with a drop-down
of marks and *Set* / *Jog* buttons), a **Laser (hold to aim)** button, the **Scaled** and **No Scaling**
buttons, **Reset Print and Cut** and a status line.

## 7. Set the first target

1. In *First Target Position* choose which mark is the first one (by default *Mark #1*). The drop-down shows
   `Mark #N (X ... Y ...)`, the position of each mark in the design. Pick one that is easy to find on the sheet.
2. Bring the head close to the printed mark:
   - **Jog** moves the head, with the laser **off**, to where the mark *should* be. It is only an estimate:
     with the first target it assumes the sheet is at the job origin.
   - Or use the jog arrows / numeric keypad.
3. Fine-tune. If your laser has no red pointer, use **Laser (hold to aim)**:
   - While you hold the mouse button the laser turns on at minimum power (3% of `$30`), with the head
     standing still. Release it and the laser turns off immediately.
   - It also turns off when you release the mouse outside the button, switch window, the machine stops being
     idle, or after 10 seconds.
   - It is only available with GRBL firmware and when `$30` has been read from the machine (reconnect if the
     program says it could not read it).
   - Wear laser safety glasses.
   - Repeat moving and looking until the dot is at the **center** of the printed mark.
4. When the head is at the center of the mark, press **Set** (or `Alt+1`).
   - The current machine position is stored and the text changes to `Mark #N set at X ... Y ...`.
   - In the preview, the mark turns red and is labeled `#N (1)`.
   - *Set* is refused with *"The machine must be connected and idle."* if the machine is not connected, is
     moving, or still has a jog queued. Wait for it to stop and try again.
   - If the chosen mark is large, you are asked *"Use this mark anyway?"* (default No).

> Pressing *Jog* again after *Set* only takes the head back to the stored position: it is not an independent
> estimate. To correct a target, move the head by hand and press *Set* again.

## 8. Set the second target

1. In *Second Target Position* choose a **different** mark (by default *Mark #2*). If you choose the same,
   aligning fails with *"The first and second target must use two different marks."*
2. *Jog* now gives a better estimate, because the first position is known and it assumes the sheet is only
   shifted. It will be close, but since the sheet is probably rotated you will need to adjust.
3. Fine-tune with the jog controls and **Laser (hold to aim)**, as in step 7.
4. Press **Set** (or `Alt+2`). The mark turns blue and is labeled `#N (2)`.

Changing the drop-down *after* pressing *Set* does not move a target that is already set. To change it, press
*Set* again.

## 9. Align (with or without scaling)

*Scaled* and *No Scaling* are enabled only when both targets are set.

**Scaled** (menu: *Align Output to Targets*; `Alt+3`) rotates, moves and **scales** the job. Use it if the
print came out slightly larger or smaller than the design (printers sometimes stretch). The scale is the
distance measured on the machine between the two marks divided by the design distance.

**No Scaling** (menu: *Align Output to Targets (no scaling)*; `Alt+4`) rotates and moves, keeping the original
size. Use it when the print is exact. It is the safest option if you doubt your measurement.

A crooked sheet only needs rotation, which both options apply. If the program asks for a large scale on a
crooked sheet, the print is probably not at 100% or a target was set off-center.

Possible warnings:

- *"The distance between the targets measured on the machine differs by X% from the design. Check that each
  target was set on the right mark and in the right order. Enable the alignment anyway?"* appears above 3%.
  It almost always means a wrong mark or the marks in the wrong order: answer **No** and check. Answer Yes only
  if the print really came out stretched.
- *"The aligned job goes outside the workspace configured in your machine..."* appears with soft limits
  enabled (`$20=1`) when the aligned job exceeds `$130`/`$131`. It could trigger an alarm or crash the head
  against the end of the axes.
- *"The two targets are too close to each other. Use two marks far apart."* (less than 1 mm).
- *"Set the first and second target positions before aligning."* (one target is missing).

If everything is fine, the wizard closes by itself (as in LightBurn) and:

- The status bar shows, in green, `Enabled Mode(s): Print and Cut`. Clicking it reopens the wizard.
- The two marks turn green in the preview.

When you reopen the wizard, the status line reads, for example:
`Print and Cut enabled (scaled): rotation 12.345 deg, scale 1.00123, second target error 0.05 mm.`

- *rotation*: how much the job was rotated.
- *scale*: scale applied (1.0 with No Scaling).
- *second target error*: how far the second mark misses, in mm, without scaling. A large value with
  *No Scaling* means the print is not exact or the aiming was off: try *Scaled* or set the targets again.

## 10. Verify before cutting

1. Look at the **3D preview**: the job is drawn already rotated, moved and scaled, and the marks move with it.
   It must sit on the printed drawing.
2. Do a **framing**. It follows the alignment, but it traces the rectangle that *encloses* the aligned job, with
   sides parallel to the axes. If the job is rotated, that rectangle extends beyond the rotated image at the
   corners (about 6.5 mm for 1.5 degrees over 247 mm). This is expected: it is not the cut outline.
3. Do a low-risk test first: scrap material with the same print, very low power or high speed, or only contours.
4. If the result is off, set the targets again (section 12), pointing more carefully.

## 11. Run or save the job

With Print and Cut enabled, **everything is aligned**: running the job (including several passes and resume
after a pause), framing, and *File → Save G-code* (the saved file already contains the alignment).

Alignment and *Reset Print and Cut* are refused while a job is running, because every pass reads the current
alignment.

Before sending:

- If the job is in inches (`G20`), **nothing is sent** and you get *"Print and Cut is enabled but this job uses
  inches (G20) and cannot be aligned..."*. Use a job in millimeters or reset Print and Cut.
- If the G-code contains commands that cannot be aligned (`G53`, `G92`/`G10`, `G28`/`G30`, `G38`, or moves
  with an unknown starting position) you get the list of those lines and *"Send the job anyway?"* (default
  No). Those commands are sent **without** alignment; usually answer No and review the G-code.

What is transformed: X and Y positions (and the I, J arc centers). Power (`S`) and speed (`F`) are untouched.
It works in absolute mode (`G90`) and also with `G91` (relative moves are rotated, not translated).

## 12. Correct, repeat and reset

- **Change a target:** press *Set* again (or `Alt+1` / `Alt+2`). The alignment is **disabled** (the green
  indicator disappears) and you must press *Scaled* / *No Scaling* again. The preview goes back to the
  unaligned job until you do.
- **Reset everything:** *Tools → Print and Cut → Reset Print and Cut*, or the button in the wizard. It has no
  shortcut by default. It clears both targets and disables the alignment.
- **Load another file:** it does **not** clear the alignment. For a different design with different marks,
  reset first.
- **Closing the program or disconnecting:** targets are not saved and are cleared when the machine is
  disconnected.
- **A new sheet with the same design:** move the sheet, *Reset Print and Cut*, and repeat sections 7, 8 and 9
  (no need to import the SVG again).

## 13. Shortcuts and menu

| Shortcut | Action |
|---|---|
| `Alt+0` | Start Wizard |
| `Alt+1` | Set First Target Position |
| `Alt+2` | Set Second Target Position |
| `Alt+3` | Align Output to Targets (scaling) |
| `Alt+4` | Align Output to Targets (no scaling) |
| *(none)* | Reset Print and Cut |

They work in the main window and with the wizard focused (the jog keys keep working). They can be changed in
*Grbl → Hotkeys*. If a shortcut collided with one you already had, the new one is left unassigned.

The *Tools → Print and Cut* menu:

| Item | Available |
|---|---|
| Start Wizard | Always |
| Set First / Second Target Position | Only with the machine connected and idle |
| Align Output to Targets (both) | Only when both targets are set |
| Reset Print and Cut | Only when there is something to clear |

Alignment items are checked when active. Menu texts are in English only for now.

## 14. Laser safety

- **Laser (hold to aim)** turns the laser **on** with the head standing still. Always use proper laser safety
  glasses.
- Power is 3% of `$30` (if `$30 = 1000`, that is `S30`). Even though it is low, it can mark sensitive
  materials (thermal paper, dark cardboard, some plastics). Do the aiming in a few seconds.
- It is designed not to stay on: it turns off on release, when the window loses focus, when the machine stops
  being idle, when the window closes, and after 10 seconds. Still, **never** walk away with the button pressed
  and never hold it down with an object.
- It uses the same command as the standard *Focus* button (needs laser mode, `$32=1`). It is refused on
  firmwares other than GRBL, and when `$30` could not be read, because the percentage would not be reliable.
- *Jog* moves the head with the laser off.

## 15. Accuracy tips

- A reasonable goal is about +-0.1 mm on a well calibrated machine. The real limit is mechanical (belts,
  steps per mm) and your aiming.
- Put the marks as far apart as possible (opposite corners).
- Use small symmetric marks (a small circle with a thin cross, a dot).
- For puzzles or pieces that fit together, compensate the kerf in the design.
- Aim at the same focus height you will cut at.
- Do not move the sheet between setting the targets and cutting.
- If the *second target error* is large with *No Scaling*, you aimed badly or the print is stretched:
  try *Scaled*.
- If the head does not come back to the same place after moving away (for example when you press *Jog* on a
  target you already set), the machine is losing steps: check belts, acceleration and jog speed.

## 16. Testing without a machine

LaserGRBL includes a GRBL emulator. The buttons are enabled as with a real machine, so you can rehearse the
whole flow safely.

- In the connection settings choose the *Emulator* protocol and connect. A *Grbl Emulator* window opens and
  LaserGRBL is connected to it.
- Import the SVG with marks, open the wizard, use *Jog* and *Set*, align, and check the colors and the status
  bar.
- Save the G-code and open it in a text editor to see the transformed coordinates.

The emulator does not tell you how the real cut will land on the material.

## 17. Messages and what to do

| Message | What to do |
|---|---|
| The machine must be connected and idle. | Connect and wait for *Idle*. Do not press *Set* while moving or with a job running. |
| Mark #N is not in the loaded job. Import an SVG with at least two registration marks. | Import the SVG again. |
| No registration marks found in the loaded job... | Check that the marks are `#FF00FF` (or within 24 levels per channel) written as `#RRGGBB`, not `rgb()`. A single mark does not count: add another. |
| Mark #N is X x Y mm: several marks may have been merged... | Marks closer than 1 mm, or a large magenta object. Separate them or fix the color. |
| The first and second target must use two different marks. | Choose different marks and press *Set* again. |
| The two targets are too close to each other... | Use marks that are farther apart. |
| ...differs by X% from the design... | Probably the mark or the order is wrong. Review. |
| ...goes outside the workspace configured in your machine... | Move the sheet, or check `$130`/`$131`. |
| ...job uses inches (G20) and cannot be aligned. Nothing will be sent. | Generate the job in millimeters or reset Print and Cut. |
| ...the following commands of the job are sent without alignment... | Review the G-code (`G53`, `G92`, `G28`...). The safe answer is No. |
| Print and Cut cannot be changed while a job is running... | Stop the job first. |
| The aiming laser needs the maximum spindle speed ($30)... | Reconnect the machine so that its configuration is read. |

## 18. Known limitations

- Marks are only detected in SVG files, not inside PNG/JPG. An image added with *Append* is aligned along with
  the rest.
- The color of the marks is fixed to `#FF00FF` (with tolerance).
- An SVG with two or more magenta marks excludes **all** its magenta (including text and images) from the
  cut/engraving. With a single mark, magenta is cut like any other color. If your design really needs magenta
  and also has marks, change the design color.
- Both marks must be in the same SVG (or the job must already have two marks from an earlier SVG). A
  single-mark SVG added with *Append* to a job without marks registers none.
- There is no object editor as in LightBurn: you choose the mark in a drop-down, not by clicking on it.
- Targets are machine positions and are not saved. Homing, a board reset or a reconnection invalidate them.
- The aligned preview is drawn only by the 3D (OpenGL) preview, not by the legacy 2D one.
- New texts are not translated yet (English only).

## 19. Quick checklist

- [ ] 1. SVG with two or more `#FF00FF` marks, far apart and symmetric.
- [ ] 2. Print at 100%.
- [ ] 3. Sheet fixed on the machine (crooked is fine).
- [ ] 4. Connect the machine (*Idle*); home it if it has homing.
- [ ] 5. *File → Open* the SVG. (If there is an image, add it **afterwards** with *Append*.)
- [ ] 6. `Alt+0`: wizard.
- [ ] 7. First mark → *Jog* → fine-tune with jog and *Laser (hold to aim)* → *Set* (`Alt+1`). Mark turns red.
- [ ] 8. Second mark → *Jog* → fine-tune → *Set* (`Alt+2`). Mark turns blue.
- [ ] 9. *Scaled* (`Alt+3`) or *No Scaling* (`Alt+4`). If it warns about more than 3%, check before accepting.
- [ ] 10. Check `Enabled Mode(s): Print and Cut` in green, the green marks and the aligned preview.
- [ ] 11. Framing and a low-risk test.
- [ ] 12. Run.
- [ ] 13. New sheet or new design: *Reset Print and Cut* and repeat.
