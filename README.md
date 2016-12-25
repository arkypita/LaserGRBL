# LaserGRBL
Laser optimized GUI for [GRBL](https://github.com/grbl/grbl/wiki)

LaserGRBL is a Windows GUI for [GRBL](https://github.com/grbl/grbl/wiki). Unlike other GUI LaserGRBL it is specifically developed for use with laser cutter and engraver.
LaserGRBL is compatible with [Grbl v0.9](https://github.com/grbl/grbl/) and [Grbl v1.1](https://github.com/gnea/grbl/)

### Download

All downloads available at https://github.com/arkypita/LaserGRBL/releases

### Existing Features

- GCode file loading with engraving/cutting job preview (with alpha blending for grayscale engraving)
- Image import (jpg, bmp...) and line by line GCode generation.
- Grbl Configuration Import/Export
- Configuration, Alarm and Error codes decoding for Grbl v1.1 (with description tooltip)
- Homing button, Feed Hold button, Resume button and Grbl Reset button
- Job time preview and realtime projection
- Jogging (for any Grbl version)
- Feed overrides (for Grbl > v1.1) with easy-to-use interface

### Missing Features

- As claim to be laser optimized Z info are ignored when drawing preview!

#### Screenshot

![Galeon](https://cloud.githubusercontent.com/assets/8782035/21349915/dba84a5a-c6b4-11e6-965f-a74fd283267a.jpg)

![Raster2Laser](https://cloud.githubusercontent.com/assets/8782035/21425748/34400d46-c84b-11e6-99e5-6eb529a98f8f.jpg)

![Alpha](https://cloud.githubusercontent.com/assets/8782035/21351296/1df460c2-c6bc-11e6-8eee-4612bb7978fa.jpg)



### Suggested GCODE Generator

We suggest to use [Inkscape](https://inkscape.org/) to draw your vector files and vectorize bitmap.

Inkscape has two interesting GCODE plugin:
- [Raster 2 Laser GCode generator](https://github.com/305engineering/Inkscape) for raster engraving
- [J Tech Photonics Laser Tool](https://jtechphotonics.com/?page_id=2012) for vector engraving/cutting

More suggestion about GCODE Inkscape extension can be found searching on [YouTube](https://www.youtube.com/results?search_query=inkscape+laser)

### Compiling

LaserGRBL is written in C# for .NET Framework 4.0 and can be compiled with [SharpDevelop](http://www.icsharpcode.net/opensource/sd/) and of course with [Microsoft Visual Studio](https://www.visualstudio.com) IDE/Compiler

### Licensing

LaserGRBL is free software, released under the [GPLv3 license](https://www.gnu.org/licenses/gpl-3.0.en.html).

### Credits

LaserGRBL contains some code from:
- [DockPanel Suite](https://github.com/dockpanelsuite/dockpanelsuite) - Copyright (c) 2007 Weifen Luo (email: weifenluo@yahoo.com).
- [ColorSlider](https://www.codeproject.com/articles/17395/owner-drawn-trackbar-slider) - Copyright Michal Brylka
- [CsPotrace](https://github.com/Invenietis/PoTrace) - Copyright Olivier Spinelli
