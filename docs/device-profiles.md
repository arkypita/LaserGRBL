# LaserGRBL-i seadmeprofiilid

## Tehniline audit

Audit põhineb LaserGRBL-i versioonil 7.14.1, upstreami commitil `1f9337b3af27`.

| Kontrollitud lähtekood | Tulemus |
| --- | --- |
| [`GrblCore.OpenFile`](https://github.com/arkypita/LaserGRBL/blob/1f9337b3af27/LaserGRBL/Core/GrblCore.cs#L641-L712), [`GrblFile.LoadImportedSVG`](https://github.com/arkypita/LaserGRBL/blob/1f9337b3af27/LaserGRBL/GrblFile.cs#L157-L189) | PNG ja JPG läbivad rastermuunduri, SVG vektormuunduri. Mõlemad annavad samasse `GrblFile.Commands` loendisse G-koodi. |
| [`ImageTransform.DitheringMode`](https://github.com/arkypita/LaserGRBL/blob/1f9337b3af27/LaserGRBL/RasterConverter/ImageTransform.cs#L151-L185), [`GrblFile.LoadImagePotrace`](https://github.com/arkypita/LaserGRBL/blob/1f9337b3af27/LaserGRBL/GrblFile.cs#L420-L463) | Mustvalge ja halltoonide töötlemine, Floyd–Steinberg, Atkinson, Jarvis ning Potrace'i täitmise ja kontuuri rajad on olemas. Blue-noise'i algoritm puudub. |
| [`MarlinCore`](https://github.com/arkypita/LaserGRBL/blob/1f9337b3af27/LaserGRBL/Core/MarlinCore.cs#L14-L43), [`GrblFile.ImageLine2Line`](https://github.com/arkypita/LaserGRBL/blob/1f9337b3af27/LaserGRBL/GrblFile.cs#L878-L925) | Marlini valik kohandab jadapordi olekupäringuid ja saatmist. Rastermuunduri `M106` haru on kommentaaris. |
| [`GrblCore.SaveProgram`](https://github.com/arkypita/LaserGRBL/blob/1f9337b3af27/LaserGRBL/Core/GrblCore.cs#L819-L853), [`GrblFile.SaveGCODE`](https://github.com/arkypita/LaserGRBL/blob/1f9337b3af27/LaserGRBL/GrblFile.cs#L58-L106) | Olemasolev `.nc` eksport kirjutab käsuloendi ning toetab üldisi algus-, vahe- ja lõpukäske ja kordusi. See ei teisenda seadme käsustikku. |

Profiilieksport loeb olemasoleva muunduri väljundit. Uus järjekord, koordinaatide ja võimsuste teisendus, eelvaade ja SD-kaardile kirjutamine asuvad eraldi `LaserGRBL/DeviceProfiles` moodulis. LaserGRBL-i tavapärane failisalvestus ja GRBL-i jadaporditöövoog on endiselt kasutatavad.

## Töö koostamine

1. Ava PNG, JPG või SVG LaserGRBL-is. Vali pildi muunduris mustvalge, halltoonid, rasterhajutus või vektorrajad. Rasterhajutuse valikutes on ka **BlueNoise**. Nimeta eri toimingute lähtefailid eraldi, kui graveerimine, voltimisjoon ja lõikamine vajavad eri seadeid.
2. Ava **File → Seadmeprofiilid ja G-koodi eksport**. Vali **Creality CV-01** või impordi enda profiil. Lisa avatud töö, GRBL-fail või testmuster. Väljal **Algtöö max. S** peab olema lähte-G-koodi võimsusskaala.
3. Määra iga rea toiming ja läbimised. Materjali seaded jõustuvad real, millel valid **Kasuta materjali**. Järjekord on graveerimine, voltimisjoon ja lõikamine. Sama toimingu read säilitavad lisamisjärjestuse. Kasuta **Kiirendusvaru** ainult horisontaalsel rasterkihil ja jäta CV-01-l tööjälje mõlemasse serva vähemalt 4 mm vaba ruumi.
4. Vali **Eelvaade ekraanil** ja kontrolli töö mõõte. Vali soovi korral **Raami seadmes laseriga** ja **Paus enne tööd**. Salvesta G-kood või kasuta **Tühjenda ja salvesta KAART**.

Testmustrid sisaldavad võimsuse ja kiiruse maatriksit, joonepaksusi, kalibreerimisruute ning 100 mm joonlauda. Joonlaual on 101 märki, pikem märk iga 5 mm ja arv iga 10 mm järel.

Materjalil on eraldi võimsus, kiirus, läbimiste arv ja katsetamise olek iga toimingu jaoks. Valitud võimsus on selle rea ülempiir. Halltoonid skaleeruvad selle ülempiiri järgi ja nullvõimsus jääb nulliks. Tundmatu materjal kuvab koostise kontrollimise hoiatuse. Sobimatuks märgitud materjali seadete rakendamine peatab ekspordi. Katsetatud väärtused kehtivad dokumenteeritud proovidele.

## Profiil ja valideerimine

Profiile saab salvestada ning importida ja eksportida JSON-failina. Kohustuslikud väljad määravad vorminguversiooni `"Version": 1`, nime, tööala, töö- ja kiirliikumise kiirused, võimsuse ülempiiri, laserikäskude paari, faililaiendi ja reavahetuse `LF` või `CRLF`. Nullpunkt, telgede peegeldus, kiirendusvaru, miinimumvõimsus, raamimine, paus ning algus- ja lõpukäsud on valikulised. `FrameByDefault` ja `PauseByDefault` määravad ekspordidialoogi vaikeseaded. Kohatäide `{power}` asendub täisarvulise väljundvõimsusega.

Lubatud väljundpaarid on `M3/M4` koos `M5` ja `M106` koos `M107` või `M106 S0`. Marlini puhul peab sisselülitus ja väljalülitus kasutama sama ventilaatori väljundit. Algus- ja lõpukäsud piirduvad ühikute, koordinaadirežiimi ning käsustikule sobiva nullimise ja sünkroonimisega. Profiilieksport ei ava jadaporti ega käivita laserit. LaserGRBL-i tavalisi käivitamise juhtnuppe tuleb kasutada eraldi ja ainult sobiva seadmega.

Lähtefaili XY-liikumised, modaalne `S` ja `F` ning `M3/M4/M5` teisendatakse selgete `G0` ja `G1` ridade kujule. `G2/G3` kaared tükeldatakse sirgeteks ja kaare äärmised punktid lisatakse tööala kontrolliks. Eksporditud koordinaadid on absoluutsed millimeetrid. Väljund lülitab laseri välja enne kiirliikumist, pausi ja lõppu. Kiirenduste tegeliku ajastuse määrab seadme püsivara.

Lähtefaili tundmatud käsud, Z-telg, koordinaadinihe `G92` ja juba teisendatud `M106/M107` väljund põhjustavad reale viitava veateate. Kui avad LaserGRBL-i **Advanced Save** abil salvestatud faili, jäta välja tavapärane `Z0` sisaldav lõpukäsk.

## Creality CV-01

Tehase CV-01 1,6 W mudeli tööala on 170 × 200 mm. Sisseehitatud profiil kasutab `M106 S0–255`, `M107`, `G0/G1`, `G21`, `G90`, `G28 X0 Y0`, kiirliikumist `F7200`, 4 mm varu ning ASCII `.gcode` faili. Töö vaikekiirus on `F1800`. Valikuline raamimine teeb kaks ringi võimsusel `S5`, valikuline `M0` lisab seadmel jätkatava pausi. Töö käivitatakse SD-kaardilt seadme nupuga. [Creality spetsifikatsioon](https://mvip.creality.com/en/goods/goodsDetail/952) · [CV-01 käsunäidis](https://cncgcode.weebly.com/creality-cv-01.html) · [CV-01 raamimise arutelu](https://forum.lightburnsoftware.com/t/save-framing-gcode-to-beginning-of-gcode/54689)

Kasutaja töötavas failis `CV01_testmaatriks_20_overscan_mm.gcode` on ASCII ja CRLF, `M107`, X/Y nullimine, kaks `S5` raamimisringi, `M0`, 4 mm väljalülitatud sisse- ja väljasõidud ning lõpus uus `M107` ja nullimine. Faili SHA-256 on `95151951feac4f4eb6e76d0b4fa00b97f05f2efa51d7da715953d1720c491ef8`. Katsetatud dekoratiivpaberi proovile on salvestatud graveerimine `S153 F1800` ja lõige `S153 F540`. Katsetatud jõupaberi proovile on salvestatud lõige `S255 F180`. Kõigil kolmel seadel oli üks läbimine.

Upstream Marlin seob tavaliselt `M106` väärtuse järgmise planeeritud liigutusega. `LASER_SYNCHRONOUS_M106_M107` muudab käitumist ja `M400` ootab liikumisjärjekorra tühjenemist. CV-01 püsivara versioon ja PWM-i täpne ajastus on kinnitamata. Töötavas näidisfailis puudub `M400` ja CV-01 profiil jätab selle vaikimisi tühjaks. Valikuline `M400` lisandub väljundvõimsuse muutmise ette ning võib rasterjoone kiirust muuta. [Marlin M106](https://marlinfw.org/docs/gcode/M106.html) · [Marlini laseri konfiguratsioon](https://marlinfw.org/docs/configuration/2.0.9/laser_spindle.html#laser-on-fan-pin) · [Marlin M400](https://marlinfw.org/docs/gcode/M400.html)

## SD-kaart

Vali Windowsi nimekirjast eemaldatav kirjutatav köide. Funktsioon tuvastab valitud köite identifikaatorid ja kontrollib neid kirjutamise eri etappides. Uus fail läheb kaardi juurkausta nimega `LASERGRB` ja profiili laiendiga. Funktsioon kirjutab ajutise faili, võrdleb kettalt loetud SHA-256 kontrollsummat, seejärel kinnitab lõppfaili. Rakendus haldab oma failide nimesid ja kontrollsummasid failis `LGRBL.SHA`. Vigane loend, muutunud kontrollsumma või võõras samanimeline fail peatab salvestamise. Ülejäänud failid jäävad alles.

Kui Windowsi lugeja lubab, lukustatakse ja eemaldatakse valitud köide pärast kirjutamist. Kui süsteem või kaardilugeja ei toeta eemaldamiskäsku või kaart on hõivatud, kuvatakse salvestuse tulemus ja palutakse kasutada Windowsi turvalist eemaldamist. Seade võib alustada SD-kaardilt esimese leitud `.gcode` faili. Kontrolli enne käivitamist, millised tööd kaardile jäid.

## Koostamine ja kontroll

Windowsis kasuta Visual Studio 2022 Build Toolsi koos MSBuildiga.

```powershell
powershell -NoProfile -File .\scripts\build-device-profiles.ps1
powershell -NoProfile -File .\tests\WindowsImageRegression\Run.ps1 -App .\artifacts\device-profiles\app\LaserGRBL.exe
```

Paigaldaja ja selle SHA-256 fail ilmuvad kausta `artifacts\device-profiles\installer`. Paigaldus lisab eraldi programmi ja kasutajaseadete kausta. `-SkipInstaller` koostab ainult rakenduse.

```sh
dotnet run --project tests/ProfileRegression/ProfileRegression.csproj --configuration Release
```

Eraldiseisev testikomplekt kontrollib profiili JSON-i, GRBL-i modaalset sisendit ja kaari, väljundi vormingut ja võimsust, tööala ning kiirendusvaru, kihte ja kordusi, raamimist, pausi, väljalülitamise järjekorda, testmustrite mõõte ning mälukaardi failide omandit ja katkestusest taastumist. Lisa argumendid `-- --cv01-reference <näidisfaili tee>`, et kontrollida ka kasutaja varem töötanud joonlauafaili. Windowsi integratsioonitestid käitavad PNG, JPG ja SVG muundamist, sealhulgas rasterhajutust ja vektorkontuure. CV-01 tegelik PWM-i ajastus ja füüsilise SD-kaardi eemaldamine vajavad seadmekatset.
