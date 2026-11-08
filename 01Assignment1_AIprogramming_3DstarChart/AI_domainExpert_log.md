# AI Domain Expert Log – Component 1: 3D Star Chart

This log contains every prompt I sent to the AI about the domain (astronomy, stars, star catalogues) and its full response. These questions were asked in a separate chat from the programming chat, so the AI acts as the "client's domain expert".

- **AI used:** Claude (Anthropic), model Claude Opus 5.5, via the Claude app chat window (separate conversation)

---

## Entry template (copy this for every prompt)

### Entry N – YYYY-MM-DD HH:MM

**Prompt:**
```
(paste my exact question here)
```

**AI response (full):**
```
(paste the AI's full response here)
```

**How I checked it / what I used it for:**
(e.g. checked against the HYG dataset or another source; used it to design X)

---

## Log


### Entry 1 – 2026-11-06 (between 00:52 and 00:57)

Asked in a separate Claude chat (Claude Opus 5.5, in my ASE project), used only for domain questions. The question was drafted for me by the programming AI (see AI_programming_log.md, Entry 53).

**Prompt:**
```
I'm building a 3D star chart using the HYG v4.4 star database. Most stars have no proper (common) name. How do astronomers name stars that don't have a proper name? Please explain:
1. Bayer designations (e.g. "Tau" + "Cet") – how they are written in full, and what suffixes like "Alp-2" mean.
2. Flamsteed numbers (e.g. 61 + "Cyg" → "61 Cygni").
3. Gliese catalogue IDs (e.g. "Gl 551", "GJ 3925A").
4. Hipparcos numbers (e.g. "HIP 1").
5. If a star has several of these, which one is normally used first?
6. The Latin genitive form of all 88 constellation abbreviations (e.g. Cet → Ceti, Cyg → Cygni, Cen → Centauri).
```

**AI response (full):**
````
Stars without proper names are identified by a set of catalogue designations. Here is how each one works and how to turn HYG fields into readable labels.

1. Bayer designations

Johann Bayer’s Uranometria (1603) labelled the stars in each constellation with Greek letters, roughly in order of brightness, though he often departed from that order. The full name is the Greek letter followed by the Latin genitive of the constellation:

Tau + Cet → Tau Ceti (τ Ceti)
Alp + CMa → Alpha Canis Majoris (α CMa, Sirius)

HYG uses three-letter abbreviations for the letters:

HYG	Letter	Name	HYG	Letter	Name
Alp	α	Alpha	Nu	ν	Nu
Bet	β	Beta	Xi	ξ	Xi
Gam	γ	Gamma	Omi	ο	Omicron
Del	δ	Delta	Pi	π	Pi
Eps	ε	Epsilon	Rho	ρ	Rho
Zet	ζ	Zeta	Sig	σ	Sigma
Eta	η	Eta	Tau	τ	Tau
The	θ	Theta	Ups	υ	Upsilon
Iot	ι	Iota	Phi	φ	Phi
Kap	κ	Kappa	Chi	χ	Chi
Lam	λ	Lambda	Psi	ψ	Psi
Mu	μ	Mu	Ome	ω	Omega

Numeric suffixes like Alp-2 mark a superscript. Sometimes one letter was given to two or more stars that are close together on the sky. Examples are α¹ and α² Capricorni, and the pairs in Librae and Centauri. These are written as superscripts:

Alp-2 + Lib → Alpha² Librae, or “Alpha-2 Librae” where you can’t use superscripts.

The superscript numbers different catalogue entries. It is not the same as the A/B letters used for components of a binary star.

Bayer also used Latin letters (e.g. “b Puppis”, “P Cygni”) once the Greek alphabet ran out. They are rarer, but they follow the same pattern.

2. Flamsteed numbers

John Flamsteed’s catalogue, published in 1725, numbered stars within each constellation from west to east by right ascension. The full name is the number followed by the genitive:

61 + Cyg → 61 Cygni
51 + Peg → 51 Pegasi

It covers only stars visible from Greenwich, so far-southern constellations have no Flamsteed numbers. Many stars have both a Flamsteed number and a Bayer letter. For example, 58 Ori is Betelgeuse, which is α Ori.

3. Gliese catalogue (Gl / GJ)

The Gliese Catalogue of Nearby Stars lists stars within about 25 parsecs of the Sun. Many nearby faint red dwarfs are known mainly by these numbers, so they matter a lot for a 3D chart.

Gl numbers come from Gliese’s original catalogue (1969). They run from 1 to 915, and decimals were used for stars inserted later, e.g. Gl 15.1.
GJ numbers are Gliese & Jahreiss additions. They use ranges such as 1001–1294, 2001–2159 and 3001–4388, e.g. GJ 1061, GJ 3925.
A trailing letter (A, B, C…) is a component of a multiple system. GJ 3925A is the primary of the GJ 3925 system.
In modern papers “GJ” is often used for all of them. Gl 551 (Proxima Centauri) is frequently written GJ 551, and both forms are acceptable.
You may also see other prefixes in HYG’s gl field, such as NN (stars with no assigned number) or Wo (Woolley). Display these as they appear.
4. Hipparcos numbers

The Hipparcos satellite catalogue (1997) contains about 118,000 stars, numbered HIP 1 to HIP 120416 in order of right ascension. The format is simply “HIP” + number, e.g. HIP 1 or HIP 70890 (Proxima). HIP numbers carry no meaning beyond being an index, but nearly every brighter star has one. HYG also includes HD (Henry Draper) and HR (Bright Star / Yale) numbers, which are written as “HD 48915” and “HR 2491”.

5. Which one comes first?

The usual order of preference, used by most atlases and planetarium software, is:

Proper name: Sirius, Proxima Centauri
Bayer: α Canis Majoris. This is preferred over Flamsteed when a star has both.
Flamsteed: 61 Cygni
Gliese: GJ 1061. For nearby stars this is usually the best-known name after the ones above, so in a nearby-stars chart rank it above HIP/HD.
HD, then HIP (or the reverse; both are common)
HYG’s own id as a last resort

A sensible labelling function is: proper || bayer+con || flam+con || gl || "HD "+hd || "HIP "+hip.

6. Genitives of all 88 constellations
Abbr	Genitive	Abbr	Genitive	Abbr	Genitive
And	Andromedae	Cyg	Cygni	Oph	Ophiuchi
Ant	Antliae	Del	Delphini	Ori	Orionis
Aps	Apodis	Dor	Doradus	Pav	Pavonis
Aqr	Aquarii	Dra	Draconis	Peg	Pegasi
Aql	Aquilae	Equ	Equulei	Per	Persei
Ara	Arae	Eri	Eridani	Phe	Phoenicis
Ari	Arietis	For	Fornacis	Pic	Pictoris
Aur	Aurigae	Gem	Geminorum	Psc	Piscium
Boo	Boötis	Gru	Gruis	PsA	Piscis Austrini
Cae	Caeli	Her	Herculis	Pup	Puppis
Cam	Camelopardalis	Hor	Horologii	Pyx	Pyxidis
Cnc	Cancri	Hya	Hydrae	Ret	Reticuli
CVn	Canum Venaticorum	Hyi	Hydri	Sge	Sagittae
CMa	Canis Majoris	Ind	Indi	Sgr	Sagittarii
CMi	Canis Minoris	Lac	Lacertae	Sco	Scorpii
Cap	Capricorni	Leo	Leonis	Scl	Sculptoris
Car	Carinae	LMi	Leonis Minoris	Sct	Scuti
Cas	Cassiopeiae	Lep	Leporis	Ser	Serpentis
Cen	Centauri	Lib	Librae	Sex	Sextantis
Cep	Cephei	Lup	Lupi	Tau	Tauri
Cet	Ceti	Lyn	Lyncis	Tel	Telescopii
Cha	Chamaeleontis	Lyr	Lyrae	Tri	Trianguli
Cir	Circini	Men	Mensae	TrA	Trianguli Australis
Col	Columbae	Mic	Microscopii	Tuc	Tucanae
Com	Comae Berenices	Mon	Monocerotis	UMa	Ursae Majoris
CrA	Coronae Australis	Mus	Muscae	UMi	Ursae Minoris
CrB	Coronae Borealis	Nor	Normae	Vel	Velorum
Crv	Corvi	Oct	Octantis	Vir	Virginis
Crt	Crateris			Vol	Volantis
Cru	Crucis			Vul	Vulpeculae

Parsing gotchas:

Abbreviations are case-sensitive: CMa, CVn and TrA are different from Cma, Cvn and Tra.
“Tau” is both a Greek letter and the abbreviation for Taurus, so always read the bayer and con columns separately and never split a combined string.
“Boötis” is often written “Bootis” without the diaeresis.
Serpens is split into Caput and Cauda on the sky, but it uses the single code Ser.

I can turn the Greek-letter and genitive tables into a ready-to-use JSON/JS lookup file plus a labelling function for HYG rows, if that would help.
````

**How I checked it / what I used it for:**
I checked the answer against my 2,000-star data file and the IAU list of 88 constellations, with help from the programming AI (AI_programming_log.md, Entry 54):
- The genitive table has exactly 88 entries, matching the IAU list (none missing, none extra), and covers every constellation in my data. Only Sol has no constellation.
- All 38 Bayer values in my data match the Greek letter table, including suffixes like Alp-2, Omi-2 and Pi-3.
- My data uses only "Gl" (1,036 stars) and "GJ" (850 stars) Gliese prefixes.
- My data has a Latin-letter Bayer star, p Eridani (Gl 66A and Gl 66B), which the answer only mentioned briefly.
- Using only the suggested labelling rule, 5 names were duplicated (61 Cyg, p Eri, Gam CrA, Eta CrB, 53 Aqr), because each is a double star. I add the A/B component letter from the Gliese ID (e.g. Gl 820A gives "61 Cygni A"), which makes all 2,000 names unique.
- **Error in the answer:** it says Flamsteed's 1725 catalogue numbered the stars. In fact the 1725 catalogue left the numbers out; the numbers used today were assigned by Lalande in his 1783 almanac (https://en.wikipedia.org/wiki/Flamsteed_designation).
- The answer puts HD before HIP. My data has no unnamed star that has an HD number but no other ID, so I did not store HD numbers.
- Not checked: the exact GJ number ranges (1001–1294 etc.). The code does not depend on them.

Used for: the Greek letter table, the 88 genitives and the naming order in CatalogueNames.cs. I used plain ASCII ("Alpha-2", "Bootis") instead of superscripts and "Boötis", because Raylib's default font cannot draw those characters.

---

### Entry 2 – 2026-11-08 (between 04:56 and 04:58)

Asked in my separate domain-expert chat ("Star naming conventions and catalogues", Claude Opus 5.5, ASE project). The question was drafted for me by the programming AI (see AI_programming_log.md, Entries 109 and 122).

**Prompt:**
```
For a 3D star chart using the HYG database (x, y, z in parsecs, Sun at the origin, +x towards the vernal equinox, +z towards the north celestial pole): when travelling from one star to another, what do "bearing" and "path" (in degrees) usually mean, and how would you calculate them from the two stars' x, y, z positions?
```

**AI response (full):**
`````
There’s no official astronomical standard for “bearing” and “path”. They come from navigation, games and science fiction rather than from astronomy. In a chart built on HYG’s equatorial x, y, z, the most natural and common meaning is a pair of spherical angles describing the direction of travel:

* Bearing is the horizontal angle, 0–360°, measured in the equatorial plane anticlockwise from +x (the vernal equinox) towards +y. It’s the same thing as right ascension, but in degrees rather than hours.
* Path (also called “mark”, “elevation” or “inclination”) is the vertical angle, −90° to +90°, above or below the equatorial plane, positive towards +z (celestial north). It’s the same thing as declination.

So “bearing 120, path 15” means: face the vernal equinox, turn 120° anticlockwise (seen from the north), then tilt 15° northwards. Because the axes don’t rotate as you move, this is simply the RA/Dec at which the destination star would appear from the starting star.
Calculation
Given start A = (x₁, y₁, z₁) and destination B = (x₂, y₂, z₂):

```
dx = x2 - x1
dy = y2 - y1
dz = z2 - z1

distance = sqrt(dx² + dy² + dz²)                    // parsecs (× 3.26156 for light years)
bearing  = atan2(dy, dx)  in degrees, wrapped to 0–360
path     = atan2(dz, sqrt(dx² + dy²))  in degrees   // same as asin(dz / distance)
```

js

```js
function course(a, b) {
  const dx = b.x - a.x, dy = b.y - a.y, dz = b.z - a.z;
  const dist = Math.hypot(dx, dy, dz);
  const deg = 180 / Math.PI;
  let bearing = Math.atan2(dy, dx) * deg;
  if (bearing < 0) bearing += 360;
  const path = Math.atan2(dz, Math.hypot(dx, dy)) * deg;
  return { dist, bearing, path };
}
```

Notes:

* Sanity check: with A as the Sun (0, 0, 0), you should get bearing = `ra × 15` and path = `dec` for star B, matching HYG’s own columns.
* Use `atan2`, not `atan`. It gets the quadrant right, and it doesn’t divide by zero when dx = dy = 0. If you’re travelling exactly along ±z, the bearing is undefined, so display it as 0 or “—”.
* The reverse trip has bearing + 180° (mod 360) and path negated.

A galactic alternative
Some science fiction and space-sim conventions measure bearing and path relative to the galactic plane, with bearing 0° pointing at the galactic centre. To use that convention, rotate (dx, dy, dz) into galactic coordinates first using the standard J2000 matrix, then apply the same two formulas:

```
gx = -0.0548755604*dx - 0.8734370902*dy - 0.4838350155*dz
gy =  0.4941094279*dx - 0.4448296300*dy + 0.7469822445*dz
gz = -0.8676661490*dx - 0.1980763734*dy + 0.4559837762*dz

bearing = atan2(gy, gx)              // galactic longitude l, 0–360
path    = atan2(gz, hypot(gx, gy))   // galactic latitude b
```

This gives galactic longitude and latitude, which feel more “physical” for a 3D map: the Milky Way’s disc becomes the horizontal plane.
If you mean a heading relative to the ship
If “bearing” should mean “how far to turn from where I’m pointing now”, you need the ship’s current orientation as forward/up/right vectors. Project the direction vector onto that frame, then use the same atan2 formulas. The total turn angle is `acos(dot(currentDir, newDir))`, using unit vectors.
For a static star chart, the equatorial version is the clearest choice and lines up with HYG’s RA/Dec columns. Use the galactic version if you want the Milky Way to be “level”.
`````

**How I checked it / what I used it for:**
I checked the answer against my 2,000-star data file (with help from the programming AI, AI_programming_log.md, Entry 124):
- Its sanity check works: computing bearing and path from the Sun with its formulas gives HYG's own right ascension × 15 and declination for all 1,999 stars, to within 0.13° (the largest difference is Gl 699.1 near the celestial pole at declination +82.8°, where small rounding in x and y makes a bigger angle difference). Examples: Proxima Centauri bearing 217.43° (RA×15 = 217.44°), path −62.68° (Dec −62.68°); Sirius 101.29° / −16.72°; Barnard's Star 269.45° / +4.69°.
- I also checked its galactic matrix: it maps the galactic centre (RA 266.405°, Dec −28.936°) to longitude 0°, latitude 0°, and the north galactic pole (RA 192.859°, Dec +27.128°) to latitude 90°, so the matrix is correct.
- The answer is honest that "bearing" and "path" have no official astronomy definition – they come from navigation, games and science fiction. The spec does not define them either, so this is a design choice.

Used for: I used the equatorial definition it recommends for a static star chart (bearing 0–360° from +x towards +y; path −90° to +90° towards +z) in Course.cs. I did not use the galactic version or the ship-relative version.

---
