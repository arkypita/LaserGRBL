# LaserGRBL
Laser optimized GUI for [GRBL](https://github.com/grbl/grbl/wiki)

LaserGRBL is a Windows GUI for [GRBL](https://github.com/grbl/grbl/wiki). Unlike other GUI LaserGRBL it is specifically developed for use with laser cutter and engraver.
LaserGRBL is compatible with [Grbl v0.9](https://github.com/grbl/grbl/) and [Grbl v1.1](https://github.com/gnea/grbl/)

### Download

All downloads available at https://github.com/arkypita/LaserGRBL/releases

### Existing Features

- GCode file loading with engraving/cutting job preview (with alpha blending for grayscale engraving)
- Image import (jpg, bmp...) with line by line GCode generation (horizontal, vertical, and diagonal).
- Image import (jpg, bmp...) with Vectorization! [Experimental]
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

LaserGRBL has its own image import, but if you need more power we suggest to use [Inkscape](https://inkscape.org/) to draw your vector files and vectorize bitmap.

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
- [Bezier2Biarc](https://github.com/domoszlai/bezier2biarc) - Copyright Laszlo

### Support and Donation

Do you like LaserGRBL? Support development with your donation!

<form action="https://www.paypal.com/cgi-bin/webscr" method="post" target="_top">
<input type="hidden" name="cmd" value="_s-xclick">
<input type="hidden" name="encrypted" value="-----BEGIN PKCS7-----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-----END PKCS7-----
">
<input type="image" src="https://www.paypalobjects.com/en_US/i/btn/btn_donateCC_LG.gif" border="0" name="submit" alt="PayPal - The safer, easier way to pay online!">
<img alt="" border="0" src="https://www.paypalobjects.com/it_IT/i/scr/pixel.gif" width="1" height="1">
</form>

You are a developer and you want to help to implement new features? Open an [issue](https://github.com/arkypita/LaserGRBL/issues) and tell me your availability.
