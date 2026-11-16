# Advanced Software Engineering
## Semester 1 2026

# Name: Kaushal Babu Yadav
# Group: Group 4 (Wednesday 9am)
# Course: MSc Information Technology
# Tutor: Dr Duncan Mullier

### [YouTube Demo Link for Assignment1](https://insertYourLinkHere)

### [YouTube Demo Link for Assignment2](https://insertYourLinkHere)


---

# Assignment 1: AI Programming – 3D Star Chart (StarChart3D)

All the code is in [`01Assignment1_AIprogramming_3DstarChart/StarChart3D`](01Assignment1_AIprogramming_3DstarChart/StarChart3D).
The logs are [`AI_programming_log.md`](01Assignment1_AIprogramming_3DstarChart/AI_programming_log.md) (every programming prompt, full response, code, explanation and critique) and [`AI_domainExpert_log.md`](01Assignment1_AIprogramming_3DstarChart/AI_domainExpert_log.md) (astronomy questions).

## My choice of AI system and approach

I used **Claude (Anthropic), model Claude Opus 5.5, through the Claude app on my Mac** as my AI programming assistant, and a **separate Claude chat as my astronomy domain expert**. I chose Claude because it can read my GitHub repository and the assignment brief, explain the code it writes in plain English, test its own code before giving it to me, and work step by step with me. I used a cloud model instead of a local LLM so that I did not have to install and run a large model on my MacBook Air. My approach was to treat the AI as a junior developer: I asked for one sub-feature at a time, ran and checked the result on my own Mac, and only then committed it with its log entry. I ran every git command myself – the AI only read my repository to check my commits. I questioned the AI's answers and recorded the bugs and misconceptions it introduced in the critique of each log entry, for example a misread data column, a click feature that only worked on the list and not on the stars, and a "working" test that was really running the old program. Domain questions (how stars are named, what "bearing" and "path" mean) were asked in the separate chat, and the AI checked the expert's answer against the real star data before using it.

## Setting up my environment

I developed on **macOS (MacBook Air)**, so I could not use the tutorial's WPF and Helix Toolkit, which only run on Windows. I used **Raylib-cs**, a cross-platform C# wrapper for the raylib graphics library, instead.

1. Install **.NET SDK 10** (I used 10.0.403) from https://dotnet.microsoft.com/download and check it with `dotnet --version`.
2. Install **Visual Studio Code** and its **C# Dev Kit** extension.
3. Clone this repo and create the project:
   ```
   cd 01Assignment1_AIprogramming_3DstarChart
   dotnet new console -n StarChart3D
   cd StarChart3D
   dotnet add package Raylib-cs --version 8.1.0
   ```
4. The star data file is in `StarChart3D/Data/` and is copied next to the program when it builds (see `StarChart3D.csproj`).
5. Run it:
   ```
   cd 01Assignment1_AIprogramming_3DstarChart/StarChart3D
   dotnet run
   ```
AI set-up: no installation is needed – I used the Claude app (claude.ai) with my account, with one chat for programming and one for domain questions.

## Star data

The stars come from the **HYG database v4.4** by astronexus (https://codeberg.org/astronexus/hyg), licensed **CC BY-SA 4.0**. The full file has 119,614 stars and 37 columns. I kept the header and the **2,000 nearest stars** (including the Sun) by sorting on the distance column:
```
(head -1 hyg_v44.csv; tail -n +2 hyg_v44.csv | LC_ALL=C sort -t, -k10,10g | head -2000) > stars_local_2000.csv
```
The farthest of these is GJ 3925A at 20.53 parsecs (about 67 light years). 89 of the stars have a proper name.

## What the program does

StarChart3D shows the 2,000 real stars nearest the Sun in 3D and lets you fly between them.

- **Feature 1 – Star chart:** reads the star file into `Star` objects held in a `StarCollection`, and gives stars without a proper name a catalogue name (Bayer, then Flamsteed, Gliese, HIP, HYG; A/B for double stars), e.g. "Alpha Centauri", "61 Cygni", "GJ 3622".
- **Feature 2 – 3D visualising:** each star is a small sphere at its real position, coloured by spectral class (O, B, A, F, G, K, M, white dwarf, unknown), with a colour key and name labels. **F** shows only named stars. Zoom, rotate and pan with the mouse (wheel, left-drag, right-drag) or keyboard (+/-, arrows, W/A/S/D/Q/E). **R** resets.
- **Feature 3 – 3D movement:** at start-up the camera flies in to the Sun; "dust" particles streak past to show motion; a list shows the 10 nearest stars with **distance (light years), bearing and path (degrees)**. Click a star, click a row, or press **1–9, 0** to fly there; on arrival the list is worked out from the new star.
- **Feature 4 – Additional (a spaceship game):** **V** switches to a spaceship (W/S throttle, X stop, arrows or mouse to steer, Q/E roll); a flight display shows heading, target, distance, bearing, path, off-course angle and arrival time (**T** or a click picks the target); **P** autopilot flies to the target and stops; **C** switches between cockpit view and a chase view of a 3D ship model.

Bearing and path follow the domain expert's definition (AI_domainExpert_log.md, Entry 2): bearing is the angle in the equatorial plane from the vernal equinox direction, `atan2(dy, dx)` (0–360°); path is the angle above or below that plane, `atan2(dz, horizontal distance)` (−90° to +90°).

## Class structure

| Class / file | Feature | What it does |
|---|---|---|
| `Program.cs` | all | Top-level program: loads the stars, opens the Raylib window and runs the main loop (reads keys, switches between chart view and ship mode, draws each frame). |
| `Star` (`Star.cs`) | 1 | One star from the HYG data: id, catalogue numbers, proper name, distance, position (x, y, z in parsecs), spectral type. `DisplayName` gives the proper name or else the `CatalogueName`. |
| `StarCollection` (`StarCollection.cs`) | 1 | Reads the CSV file by column name into a list of `Star`s; implements `IReadOnlyList<Star>`; `FindById`; keeps any skipped lines. |
| `CatalogueNames` (static) | 1 | Builds a catalogue name for stars without a proper name (Bayer → Flamsteed → Gliese → HIP → HYG), with Greek letters, constellation genitives and A/B components. |
| `SpectralClass` (enum) and `SpectralClasses` (static) | 2 | The spectral classes; `Parse` reads the class from the spectral type text; `ColourOf` and `NameOf` for drawing and the key. |
| `StarRenderer` | 2, 3, 4 | Draws the stars, labels and colour key; the named-star filter; converts between HYG and Raylib coordinates; `StarAt` finds the star under the mouse; `StarNearestDirection` finds the star at the crosshair. |
| `CameraController` | 2, 3 | The orbit camera (distance, yaw, pitch, target): zoom/rotate/pan from mouse and keyboard, the animated `FlyTo` with easing, `Reset` and `LookFrom`. |
| `DustField` | 3 | 400 faint particles in a box around the camera that streak past when the camera moves. |
| `Course` (record struct) | 3, 4 | Distance, bearing and path from one star (or any point) to another; `ToNearest` finds the nearest stars. |
| `NearbyStarsPanel` | 3 | The list of the 10 nearest stars to the current location; selecting a row by click or number key; recalculates when the location changes. |
| `ShipController` | 4 | The spaceship: position, quaternion orientation, throttle and speed, steering and roll, autopilot, cockpit and chase cameras. |
| `FlightHud` (static) | 4 | The flight display: crosshair, target marker or edge arrow, heading, distance, bearing, path, off-course angle, arrival time and autopilot status. |
| `ShipModel` (static) | 4 | Draws the 3D spaceship from triangles, with an engine glow that grows with speed. |

How they fit together: `Program` creates a `StarCollection` (made of `Star`s, named by `CatalogueNames`), a `StarRenderer`, a `CameraController`, a `DustField`, a `NearbyStarsPanel` (which uses `Course`) and a `ShipController`. Each frame it updates either the camera or the ship, then draws the stars, dust, ship model, labels and panels.

## Branches and pull requests

| Feature | Branch | Commits | Pull request (merge message) |
|---|---|---|---|
| 1 | `feature1-create-star-chart` | 4 | #1 "Feature 1 Star Chart" |
| 2 | `feature2-visualising-star-chart` | 6 | #2 "Feature 2 3D Visualising the Star Chart" |
| 3 | `feature3-3d_movement` | 4 | #3 "Feature 3 3D movement" |
| 4 | `feature4-additional-facilities` | 4 | #4 "Feature 4 Additional" |

## Known limitations

Stars and the ship are drawn much larger than real scale so they can be seen; ship speeds are far faster than light so trips take seconds; there is no collision, so you can fly through stars; the autopilot flies in a straight line; errors in the HYG data are kept as they are (e.g. Capella is listed as class M1).

## References

- Hipparcos, Yale, Gliese (HYG) database v4.4, astronexus, CC BY-SA 4.0 – https://codeberg.org/astronexus/hyg
- Raylib-cs 8.1.0 (zlib licence) – https://github.com/ChrisDill/Raylib-cs
- Claude (Anthropic), Claude Opus 5.5, used as AI programming assistant and domain expert – all use recorded in the two log files.

---

Feel free to delete any of the information below to tidy up your readme, but do not delete the above information as it is used for marking and do not delete any files in the repo as some are used for building documentation for your assignments.


[Details on how to create a project in these repo directories.](https://github.com/LeedsBeckettUniversityASE/ASEresources/blob/main/VisualStudioProjects.md)

## Assignments

You must be logged into myBeckett to view the assignment spec. Sync your browswer with your LBU account.

[Assignment Specification](https://leedsbeckett-my.sharepoint.com/:b:/g/personal/d_mullier_leedsbeckett_ac_uk/IQAfMN9DgVEdQJIvbNKtkIhCAe4herw6o4nj_Iun4tXZeNU?e=JiytOR)

**See The Assignment Specification for assignment dates**. They are not repeated here becuase software engineers do not repeat things as this leads to bugs.
### [Useful Links for the Assignments](https://github.com/LeedsBeckettUniversityASE/ASEresources/blob/main/UsefulLinks.md).
These links are likely to be updated as more information is added, so check reqularly.
## Assignment 1: AI Programming 3D Star Chart
See the assignment spec for details. You are expected to implement a 3D star chart using AI programming techniques. The star chart should allow users to visualise stars in a three-dimensional space, with features such as zooming, rotating and filtering based on star properties.

You must explain your code in your demo or you will be asked to do it again/a live demo.

Edit the YouTube Link above, the part in the square brackets is the text you see and doesn't need to change. Put the url of your demo inside the round brackets and check it clicks through to your demo.

## Assignment 2: BOOSE Programming Language interpreter
See the assignment spec for details. You are expected to implement an interpreter for the BOOSE programming language. The interpreter should be able to parse and execute BOOSE code, handling various constructs such as variables, loops, conditionals, and functions.

You must explain your code in your demo or you will be asked to do it again/a live demo.

Edit the YouTube Links above, the part in the square brackets is the text you see and doesn't need to change. Put the url of your demo inside the round brackets and check it clicks through to your demo.

# Submitting Assignments
This applies to **both** assignments.

**You MUST do all of these steps or you will lose marks**. All details are in the **Assignment Specification**.

Apologies if this all seems a bit patronising but every year I get a handful who take longer to process than everyone else combined because they don't read instructions.
### All code must be part of the Classroom50 Classroom, using the this template repo. You should not commit after the deadline.
### You must provide a YouTube Demo that is uploaded as public but unlistable and the link must be provided in this README.md file above (edit the link). 
The demo should showcase the functionality of your assignments and demonstrate your understanding of the code. You must use a microphone and explain your code. 
See Assignment Specification and [YouTube demo software document](https://github.com/LeedsBeckettUniversityASE/ASEresources/blob/main/Recording_A_YouTube_Demo.md)
### You MUST submit links to your video, repo and upload your source code directory to **myBeckett**, this is the official submission that records your hand in time, 
If you do not do this on time it is officially not handed in/late.

[University's Flexible Submission Policy](https://drive.google.com/file/d/108-9vWaaakjDZKFBI6SffSGaSzmYbNJk/view?usp=sharing)

[University's Extenuating Circumstance Policy](https://www.leedsbeckett.ac.uk/student-information/exams-assessments-and-awards/mitigation-and-extenuating-circumstances/)

**Module Leaders cannot give out extensions, so don't ask, instead click the above link**

