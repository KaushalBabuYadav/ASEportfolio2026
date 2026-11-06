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
