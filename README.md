# LaserGRBL
Laser optimized GUI for [GRBL](https://github.com/grbl/grbl/wiki)

LaserGRBL is a Windows GUI for [GRBL](https://github.com/grbl/grbl/wiki). Unlike other GUI LaserGRBL it is specifically developed for use with laser cutter and engraver.
LaserGRBL is compatible with [Grbl v0.9](https://github.com/grbl/grbl/) and [Grbl v1.1](https://github.com/gnea/grbl/)

#### Download

[Download precompiled version of LaserGRBL v0.1](https://dl.dropboxusercontent.com/u/21662123/shared%20on%20web/LaserGRBL.exe)

#### Existing Features

- File loading with engraving/cutting job preview (with alpha blending for grayscale engraving)
- Grbl Configuration Import/Export
- Configuration, Alarm and Error codes decoding for Grbl v1.1 (with description tooltip)
- Homing button, Feed Hold button, Resume button and Grbl Reset button
- Job time preview and realtime projection

#### Missing Features

- Jogging
- Feed overrides (added in Grbl v1.1)

#### Known Bugs

- 2D preview works only with absolute positions (G91 not supported in preview)
- XYZ commands without explicit G command are ignored building preview
- As claim to be laser optimized Z info are ignored when drawing preview

#### Licensing

LaserGRBL is free software, released under the [GPLv3 license](https://www.gnu.org/licenses/gpl-3.0.en.html).

LaserGRBL is written in C# for .NET Framework 4.0 and can be compiled with [SharpDevelop](http://www.icsharpcode.net/opensource/sd/) and of course with [Microsoft Visual Studio](https://www.visualstudio.com) IDE/Compiler

#### Screenshot

![Galeon](https://cloud.githubusercontent.com/assets/8782035/20867453/e59430e6-ba44-11e6-91ed-0d3407387adf.jpg)

![Alpha](https://cloud.githubusercontent.com/assets/8782035/20867452/e58f7f38-ba44-11e6-80e1-490ae3be0358.jpg)
