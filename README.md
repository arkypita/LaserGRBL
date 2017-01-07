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

#### Screenshot

![Galeon](https://cloud.githubusercontent.com/assets/8782035/21349915/dba84a5a-c6b4-11e6-965f-a74fd283267a.jpg)

![Raster2Laser](https://cloud.githubusercontent.com/assets/8782035/21425748/34400d46-c84b-11e6-99e5-6eb529a98f8f.jpg)

![Alpha](https://cloud.githubusercontent.com/assets/8782035/21351296/1df460c2-c6bc-11e6-8eee-4612bb7978fa.jpg)


### Support and Donation

Do you like LaserGRBL? Support development with your donation!

[![Donate](https://www.paypalobjects.com/en_US/i/btn/btn_donateCC_LG.gif)](https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=mlpita%40bergamo3%2eit&lc=US&item_name=LaserGRBL&item_number=lasergrbl&currency_code=EUR&bn=PP%2dDonationsBF%3abtn_donateCC_LG%2egif%3aNonHosted)





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



