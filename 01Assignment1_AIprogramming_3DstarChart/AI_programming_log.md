# AI Programming Log – Component 1: 3D Star Chart

This log contains every programming prompt I sent to the AI and its full response (including all code generated), in the order they happened, with my explanation and critique of each response.

- **AI used:** Claude (Anthropic), model Claude Opus 5.5, via the Claude app (macOS)
- **Development environment:** macOS (MacBook Air), VS Code, .NET SDK 10.0.403, Raylib-cs 8.1.0
- **Domain (astronomy) questions** are kept separately in `AI_domainExpert_log.md`

Note: screenshots I sent the AI are described in brackets, as images cannot be pasted into this file.

---

## Entry template (copy this for every prompt)

### Entry N – YYYY-MM-DD HH:MM – Branch: `branch-name`

**Prompt:**
```
(paste my exact prompt here)
```

**AI response (full, including code):**
````
(paste the AI's full response here)
````

**What I did with it:**

**Explanation of the code (in my own words):**

**Critique – bugs, flaws or misconceptions the AI introduced:**

**Commit:**

---

## Log

### Entry 1 – 2026-11-05 02:17 – Branch: `main` (no code)

**Prompt:**
```
CONNECT TO GITHUB ????/
```

**AI response (full):**
````
You're already connected to GitHub as **KaushalBabuYadav**, and I can see your 7 repos with push access:

- **ASEportfolio2026** (last updated 17 Oct)
- KAUSHALBABUYADAV
- BLOCKCHAIN-SOCIAL-NETWORK-NFT
- HEALTHCARE-RECORDS-PRIVACY-DB
- CALCULATOR-PYTHON
- SNAKEGAME-PYTHON
- TICTACTOEGAME-PYTHON

Since this chat is in your ASE project, do you want me to open ASEportfolio2026 and start working on it? Tell me which repo and what you need done.
````

**What I did with it:** Nothing to commit. Confirmed the AI could see my GitHub account.

**Critique (my own words):**

---

### Entry 2 – 2026-11-05 02:21 – Branch: `main` (no code)

**Prompt:**
```
I WANT YOU TO FOCUS ON ASEPORTFOLIO2026
MAINLY ON COMPONENT 1
MAKE SURE YOU WILL READ EACH AND EVERY LINE -> DOUBLE CHECK IT
REMAINING PART I WILL PROVIDE YOU IN NEXT PROMPT
IF YOU HAVE ANY DOUBT THEN FEEL FREE TO ASK ME
NOTE: NO HALLUCINATION
```

**AI actions:** The AI made a read-only copy (clone) of my repo and read every file.

**AI response (full):**
````
I've cloned **ASEportfolio2026** and read every line of every file (548 lines across 5 files, checked twice). Here's exactly what's in it, and nothing more:

**What the repo contains**
- It's the untouched Leeds Beckett ASE template: one commit, "Initial commit".
- `01Assignment1_AIprogramming_3DstarChart/`, `02Assignment2_BOOSE_ProgrammingLanguage/` and `03LabWork/` are **empty** (each holds only a placeholder `.gitkeep` file). There is no code yet.
- `README.md` still has placeholders: `[Your Name]`, `[Your Group]`, `[Your Course]`, and both YouTube links point to `https://insertYourLinkHere`. It says filling these in correctly is part of the marking.
- `docfx.json` builds API docs from the `.csproj` files in the two assignment folders, so both assignments are expected to be C# / .NET projects. Its title still says `[put my name here]`.
- `index.md`, `toc.yml` and `.gitignore` are standard template files (Visual Studio ignore rules).
- Assignment 1 is described only briefly: a 3D star chart that lets users view stars in 3D, with zooming, rotating and filtering by star properties, and you must explain your code in a YouTube demo.

**Things I need to check with you**
1. **The word "Component 1" doesn't appear anywhere in the repo.** Does it mean Assignment 1 (the 3D star chart), or a section of the assignment specification? The spec is a SharePoint PDF behind your university login, so I can't open it. Please paste or attach the spec (or the Component 1 part of it).
2. Should I fill in the README now? If so, send me your name, group and course exactly as they should appear.

**One limitation:** I can read this repo, but pushing changes from here is currently refused because the Claude GitHub App doesn't have write access to it. If you want me to commit directly, install it at https://github.com/apps/claude/installations/select_target. Until then, I can prepare the files and you push them yourself.

Send the next part whenever you're ready.
````

**What I did with it:** Nothing to commit.

**Critique (my own words):**

---

### Entry 3 – 2026-11-05 02:23 – Branch: `main` (no code)

**Prompt:**
```
https://github.com/KaushalBabuYadav/ASEportfolio2026/tree/main/01Assignment1_AIprogramming_3DstarChart
```

**AI actions:** The AI fetched the latest version of the repo (read-only) and listed the folder.

**AI response (full):**
````
I've re-checked GitHub just now, and that folder is still **empty**:

- `main` is the only branch, and it still has only the "Initial commit".
- `01Assignment1_AIprogramming_3DstarChart/` contains only `.gitkeep`, which is a 0-byte placeholder file. There's no code, no `.csproj`, and no notes in it.

So there's nothing for me to read in that folder yet. The likely reasons:

1. **Your code is only on your computer** and hasn't been pushed yet. You can either push it, or attach the project folder here (or add it with "Add folder" from the **+** menu in the Claude desktop app) and I'll read it directly.
2. **You haven't started yet** and want me to help build Assignment 1 from scratch. In that case I need the **assignment specification** (the Component 1 part at least). It's on SharePoint behind your university login, so please paste the text or attach the PDF here.

Which one is it?
````

**What I did with it:** Nothing to commit.

**Critique (my own words):**

---

### Entry 4 – 2026-11-05 02:26 – Branch: `main` (no code)

**Prompt:**
```
I am not giving you premission to do any change of github such push and pull
till now i didnt starrt anything
i am adding some pdf so you can understand what we need to do
```

**AI response (full):**
````
Understood. I won't push, pull, commit or change anything on GitHub. My copy of the repo stays read-only on my side, and nothing leaves this session without your say-so.

I don't see any PDFs in this message yet; they may still be uploading or didn't attach. Please send them, and I'll read every page line by line, double-check it, and stick strictly to what's written in them. Anything unclear, I'll ask you instead of guessing.
````

**What I did with it:** Added the assignment PDF (`2026ASEAssignment.pdf`) to my Claude project so the AI could read it.

**Critique (my own words):**

---

### Entry 5 – 2026-11-05 03:21 – Branch: `main` (no code)

**Prompt:**
```
I ALREADY CONNECTED ASEportfolio2026
```

**AI actions:** The AI read the full assignment PDF from my Claude project.

**AI response (full):**
````
I've read the whole PDF (all pages, every line), and checked it a second time against the text. Here's Component 1 exactly as the spec states it, with nothing added.

## Component 1 at a glance
- **Title:** AI Programming, **40%** of the module, suggested effort **50 hours**.
- **Deadline: Monday 9/11/2026, 12pm** (Week 7). Today is Thursday 5 Nov, so that's about **4 days away**, and nothing has been started.
- **What to build:** a realistic 3D star chart and space simulation, made with AI as your "junior developer". It must be done the software-engineering way: critique the AI's output and control every change with version control. It explicitly says this is **not vibe programming**.
- **Any AI is allowed** (Gemini, ChatGPT, Claude, or a local LLM).
- You're meant to work through the module leader's AI-built example/tutorial, but **not copy it**. Similar code will be investigated for plagiarism.

## For every feature, the same three things
1. **Log every prompt and full response** with the AI. The log will itself be checked by AI against your code.
2. **Explain and critique the code.** A first-class mark needs a clear critique of bugs, flaws or misconceptions the AI introduced. Full functionality with no critique gets **no marks**.
3. **Version control:** one branch per feature, one commit per sub-feature with clear messages, and a pull request to main when each feature is done. The log entries must be written **at the time of each commit**; timestamps are checked against the commit history, and you "cannot forge a version control history".

## Marks breakdown (100)
**Setting Up (20)**
- README fully filled in: author, group, **tutor**, YouTube link.
- A paragraph on which AI you chose and why.
- Environment setup instructions (which model/version, local or cloud).
- `AI_programming_log.md`: programming prompts and full responses, including all code.
- `AI_domainExpert_log.md`: prompts about astronomy and stars.
- At the end: a full project description and class structure in the README.

**Feature 1: The Star Chart (15)**, branch `feature1-create-star-chart`
- Read real local star data (**at least 2000 stars**) into `Star.cs`.
- Generate catalogue names for stars without proper names.
- `StarCollection.cs` that reads the star data file.
- Pull request to main with the commit message **"Feature 1 Star Chart"**.

**Feature 2: 3D Visualising (15)**, branch `feature2-visualising-star-chart`
- Render stars as points or small shapes, with names, and in spectral class colour (or otherwise differentiated).
- Filter out stars without proper names.
- Zoom, pan and scan with mouse and keyboard.
- Pull request to main.

**Feature 3: 3D Movement (20)**, branch `feature3-3d_movement`
- Must animate: 3D animation towards an initial star system.
- "Dust animation" to give the illusion of slow motion.
- List of 5 or 10 local stars; clicking one animates to it and updates the list.
- Show distance (light years), bearing (degrees) and path (degrees).
- Pull request to main with the commit message **"Feature 3 3D movement"**.

**Feature 4: Your Own Extensions (30)**, branch `feature4-additional-facilities`
- Must be "very substantial". The spec suggests ideas like comparing local LLMs, or making it more like a 3D game (Elite-style flying, spaceship models).
- Pull request to main with the commit message **"Feature 4 Additional"**.

## To be marked at all, you must do all four
1. Have your code in the repo in the ASE Classroom50 Classroom, with **no commits after the deadline**.
2. Record a **YouTube demo** that shows the Google Demo Form. You must explain your code and **declare your AI use** in it.
3. **Submit the Google Demo Form**; you'll get an email receipt.
4. Hand in the repo link, YouTube link **and your source code directory** on MyBeckett.

If you're late (without mitigation), the mark is capped at 40% if handed in within 10 working days, and zero after that.

## Things in the PDF I need you to clarify
1. **The tutorial link is missing.** The PDF says "The tutorial to follow is here", but the link didn't come through in the text. Can you send it, or the example from myBeckett?
2. **Language and 3D library.** The spec doesn't name one. Your repo's `docfx.json` expects `.csproj` files, so it should be **C# / .NET**. Did your module say which 3D framework to use (for example the one in the tutorial)?
3. **Star data.** The spec only says "real local star data, at least 2000 stars". Has your tutor given you a specific data file? If not, I'd suggest one, but I'd rather check than assume.
4. **"Because there are two of you working on it."** Is this just meaning you plus the AI, or do you have a partner? The cover sheet says it's an individual assessment.
5. **AI log file name.** The spec uses both `AI_programming_log.md` and `AI_log.md`. I'd use `AI_programming_log.md`, since that's the one listed under Setting Up.

## Two important honesty points
- **This conversation counts as AI use.** Every prompt you send me about the project, and my full responses, must go into your AI logs, and you need to declare it in the demo.
- **You'll do every commit, branch and pull request yourself**, as you've asked. That's also what the spec needs: each log entry and critique should be written at the time of the commit, in your own words, so the history is genuine.

Once you've answered the questions above, we can start with the **Setting Up** section. Would you also like me to save this requirements checklist in your ASE project so you can tick items off?
````

**What I did with it:** Used it as my checklist for Component 1.

**Critique (my own words):**

---

### Entry 6 – 2026-11-05 03:23 – Branch: `main` (no code)

**Prompt:**
```
1. sure
2. 2. yeahhh
```

**AI response (full):**
````
Got it: you'll send the tutorial link, and it's **C# / .NET**.

Just to be sure I read you right, since your "1" and "2" could match different questions:
- **"1. sure"** → you'll send the tutorial link? (Or did you mean "sure, save the checklist to the project"?)
- **"2. yeahhh"** → yes, C# / .NET?

And I still need these before we can start the Setting Up section:
1. **The tutorial link** from the PDF (it didn't come through in the text).
2. **Which 3D framework**, if your module or the tutorial names one. If not, I'll suggest options and you choose.
3. **Star data:** has your tutor given you a specific file, or should I suggest one?
4. **Partner:** are you working alone, or with someone?
````

**Critique (my own words):**

---

### Entry 7 – 2026-11-05 03:25 – Branch: `main` (no code)

**Prompt:**
```
now read even github you can see
so make sure you will dopropelry
```

**AI actions:** The AI re-checked my repo (read-only), then read the module's public resource repos on GitHub (LeedsBeckettUniversityASE/ASEresources and LeedsBeckettUniversityASE/SEsupportMaterial), including the full AI programming tutorial. It said it deliberately did not open the module leader's example code repo.

**AI response (full):**
````
I've now read everything, line by line: the module resources repo (5 files) and the three Assignment 1 documents in the support material repo, including the full tutorial (574 lines; I stripped out the embedded images so I could read all of the text). I haven't changed anything on GitHub.

This answers almost all of my earlier questions.

## What's now confirmed (with sources)
| Question | Answer | Source |
|---|---|---|
| Tutorial link | `SEsupportMaterial/VisualStudioAIprogramming.md` | UsefulLinks.md, line 16 |
| Language and framework | C#, **WPF** project (not the .NET Framework one), **.NET 10** or later | Tutorial lines 119–120; VisualStudioProjects.md line 26 |
| 3D library | The tutor used **Helix Toolkit** for 3D movement and rotation | Tutorial lines 255–257 |
| Star data | **HYG dataset**, `hyg_v42.csv.gz` from Codeberg. The tutor later resampled it to **2000 stars, keeping unnamed ones** | Tutorial lines 107 and 403 |
| "Two of you" | Means **you and the AI**, not a partner | UsingGitHubWIthAI.md, line 2 |
| Where the project goes | Create it inside `01Assignment1_AIprogramming_3DstarChart` | VisualStudioProjects.md lines 20–24 |
| Separate AI chats | One for code, one for astronomy, never mixed | Tutorial lines 30–31 |
| Branches | **Keep all of them**; the tutor wants to see them | UsingGitHubWIthAI.md, line 67 |
| Demo video | Unlisted (not private), chapter markers for each form question, audio, show the form | Recording_A_YouTube_Demo.md |

## Where the tutorial and the spec differ (follow the spec)
- **Branch names:** the tutorial uses `feature-create-star-class`; the spec requires `feature1-create-star-chart`, `feature2-visualising-star-chart`, `feature3-3d_movement`, `feature4-additional-facilities`.
- **PR commit messages:** the spec requires exact wording ("Feature 1 Star Chart", and so on).
- **Order:** in the tutorial, catalogue names came late; in the spec they're part of Feature 1.
- **Do not copy the tutor's code.** The spec warns that similar code will be investigated. I deliberately haven't opened his example repo (`dmullier/3DstarChart`), so nothing I write can be influenced by it.

## A realistic plan for 4 days (deadline Mon 9 Nov, 12pm)
1. **Thu:** Setting Up (20 marks): WPF project, README, AI choice paragraph, the two log files, download the data.
2. **Fri:** Feature 1 (15)
3. **Sat:** Feature 2 (15)
4. **Sun:** Feature 3 (20)
5. **Mon morning:** demo video, Google Form, MyBeckett hand-in. Feature 4 (30) only if time allows; it has to be "very substantial", so don't sacrifice the hand-in for it.

**How we'd work, so it's honest and marks well:** you send a prompt here, I reply with code, you paste **both** into `AI_programming_log.md`, then build and run it in Visual Studio, write your critique **in your own words**, and commit. Astronomy questions go in a **separate chat** and into `AI_domainExpert_log.md`. One limitation: I can't compile or run WPF here (it's Windows-only), so you'll be the one running and debugging. That's also exactly what the spec wants you to show.

## I need from you
1. **Do you have Windows with Visual Studio** (and .NET 10) installed?
2. **Which AI will you declare** as your main tool: me (Claude, in this chat), GitHub Copilot, or something else?
3. **For the README:** your full name, group, course and **tutor's name**.
````

**What I did with it:** Used it to understand the tutorial and the differences between the tutorial and the spec.

**Critique (my own words):**

---

### Entry 8 – 2026-11-05 03:27 – Branch: `main` (no code)

**Prompt:**
```
from github, i just clone to my desktop that all
why are being so confused
```

**AI response (full):**
````
Sorry, I made that more complicated than it needed to be. To put it simply:

- You've cloned **ASEportfolio2026** to your desktop, and nothing has been started yet. That matches what I see.
- I've read the assignment PDF and the tutorial, so I know exactly what Component 1 needs.
- I won't push or pull anything.

The next step is the **Setting Up** part: creating the WPF project inside `01Assignment1_AIprogramming_3DstarChart`, filling in the README, and creating `AI_programming_log.md` and `AI_domainExpert_log.md`.

To start that, I just need your **name, group, course and tutor's name** for the README.

If you'd like me to see the folder on your desktop directly, add it with **"Add folder"** from the **+** menu in the Claude desktop app. I'll only read it unless you tell me to change something.
````

**Critique (my own words):**

---

### Entry 9 – 2026-11-05 03:27 – Branch: `main` (no code)

**Prompt:**
```
we will update read.me file later
so lets starr
```

**AI response (full):** (the AI also sent me two starter files: `AI_programming_log.md` and `AI_domainExpert_log.md`)
````
Here's Setting Up, step by step. Do these on your computer, in order.

**Step 1: Create the WPF project** (from the tutorial and VisualStudioProjects.md)
1. In Visual Studio, open your cloned repo folder: **File → Open Folder → ASEportfolio2026**.
2. **File → New → Project** → choose **WPF Application** (C#). Don't pick the one that says ".NET Framework".
3. **Location:** your `01Assignment1_AIprogramming_3DstarChart` folder.
4. **Name:** something of your own, for example `StarChart3D` (avoid the tutor's name `3DstarChart`).
5. **Framework:** **.NET 10.0 (Long Term Support)**. If you don't see 10.0, update Visual Studio first.
6. Press **F5** to check the empty window opens.

**Step 2: Add the two log files**
- Download `AI_programming_log.md` and `AI_domainExpert_log.md` and put both inside `01Assignment1_AIprogramming_3DstarChart`. Keeping them in that folder stops them clashing with the log you'll need later for Component 2.

**Step 3: Log this conversation**
- The spec needs a **complete** log of every interaction, so copy your prompts from this chat and my replies into `AI_programming_log.md` as the first entries.
- From now on, open a **separate Claude chat** for astronomy questions only, and log those in `AI_domainExpert_log.md`.

**Step 4: First commit (on `main`)**
- In Visual Studio's **Git Changes** tab, check that `bin` and `obj` folders are **not** listed (your `.gitignore` already excludes them).
- Suggested commit message: `Setup: add WPF .NET 10 project and AI log files`
- Then push. You're doing the commits yourself, as you asked.

**Step 5: Create the Feature 1 branch**
- **Git → New Branch** → name it exactly `feature1-create-star-chart`.

Tell me once that's done (or if any step gives an error), and we'll start Feature 1: downloading the HYG star data and writing `Star.cs`.
````

**What I did with it:** Tried to follow it, but I am on a Mac using VS Code, not Visual Studio on Windows (see next entry).

**Critique (my own words):**

---

### Entry 10 – 2026-11-05 03:36 – Branch: `main` (no code)

**Prompt:**
```
which frame =work to choose
```
[Screenshot: VS Code on my MacBook, searching the Extensions Marketplace for ".NET 10.0 (Long Term Support)", with an empty file called "StarChart3D" open]

**AI actions:** The AI web-searched and checked that WPF is Windows-only and that the Raylib-cs NuGet package supports macOS.

**AI response (full):**
````
Important finding from your screenshot: you're on a **Mac with VS Code**, and the tutorial's framework, **WPF, only runs on Windows**. Microsoft's position is that [WPF is Windows-only, with no plans to make it cross-platform](https://github.com/dotnet/core/issues/2144). So following the tutorial exactly on your Mac won't work. Also, ".NET 10 (Long Term Support)" isn't an extension. It's the .NET SDK, which you install from Microsoft's website, so you can stop searching the Marketplace.

You have two options:

**Option A: Use a Windows PC** (for example the university labs, which have Visual Studio)
- Follow the tutorial's setup: WPF + .NET 10 + Helix Toolkit.
- Downside: you can only work when you're at a Windows machine, with 4 days left.

**Option B (my recommendation): Stay on your Mac and use Raylib-cs**
- It's a C# 3D library that [targets .NET 8 and .NET 10 and supports macOS on both Apple Silicon (osx-arm64) and Intel (osx-x64)](https://www.nuget.org/packages/Raylib-cs/8.1.0), as well as Windows and Linux.
- It has what the spec needs built in: a 3D camera, drawing points and spheres, text labels for star names, and mouse and keyboard input.
- It's a normal `.csproj`, so it works with your repo's `docfx.json`.
- Bonus: your code will be naturally different from the tutor's WPF version, which matters because the spec warns about code that's similar to his. The spec allows any approach; it says not to follow the example blindly.
- Things to know: the tutorial's Visual Studio-only tools (XAML, Helix, AI Studio extension) won't apply. You'll explain in your README why you chose Raylib-cs on macOS, which is good material for the "choice of environment" marks.

Also, in your screenshot there's a file called **StarChart3D** marked **U** (untracked). It's an empty text file, not a project, so delete it before we start.

**Which option do you want, A or B?** If B, first run this in your VS Code terminal and send me the output:
```
dotnet --version
```
````

**What I did with it:** Chose Option B (Raylib-cs on macOS).

**Critique (my own words):**

---

### Entry 11 – 2026-11-05 03:43 – Branch: `main` (no code)

**Prompt:**
```
do i have to download it Option B (my recommendation): Stay on your Mac and use Raylib-cs
* It's a C# 3D library that targets .NET 8 and .NET 10 and supports macOS on both Apple Silicon (osx-arm64) and Intel (osx-x64), as well as Windows and Linux.
```

**AI response (full):**
````
No, you don't download Raylib-cs yourself. It's a NuGet package, so one terminal command adds it to your project and .NET fetches it automatically.

The only thing you might need to download is the **.NET 10 SDK** itself, if it isn't installed yet. Here's the order:

**1. Check if .NET is installed.** In your VS Code terminal, run:
```
dotnet --version
```
- If it prints a number starting with **10**, you're ready; skip to step 2.
- If it says `command not found` or shows a lower number, download the **.NET 10 SDK for macOS** from Microsoft's .NET download page (dotnet.microsoft.com). Choose **Arm64** if your Mac has an Apple chip (M1/M2/M3/M4), or **x64** if it's Intel. Install it, then close and reopen VS Code and run the command again.

**2. Create the project** (in the terminal, inside `01Assignment1_AIprogramming_3DstarChart`):
```
dotnet new console -n StarChart3D
cd StarChart3D
```

**3. Add Raylib-cs** (this is the "download", done for you):
```
dotnet add package Raylib-cs
```

**4. Check it builds:**
```
dotnet build
```

Do step 1 first and send me what it prints. Then we'll do steps 2 to 4 together, and I'll give you a small test window to prove Raylib-cs works on your Mac before we start Feature 1.
````

**Critique (my own words):**

---

### Entry 12 – 2026-11-05 03:45 – Branch: `main` (no code)

**Prompt:**
```
.net by microsoft?
```

**AI response (full):**
````
Yes, .NET is made by Microsoft. But don't install the **".NET Install Tool"** extension from your screenshot; get the SDK directly from Microsoft's official page:

**[Download .NET 10.0](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)**

On that page:
1. Find the **SDK** section (not "Runtime").
2. Under **macOS**, click the **Installer**:
   - **Arm64** if your Mac has an Apple chip (M1, M2, M3, M4). To check, click the Apple menu → **About This Mac** → look at "Chip".
   - **x64** only if it says "Processor: Intel".
3. Open the downloaded `.pkg` file and install it.
4. **Quit and reopen VS Code**, then run in the terminal:
```
dotnet --version
```

Send me what it prints.
````

**Critique (my own words):**

---

### Entry 13 – 2026-11-05 03:46 – Branch: `main` (no code)

**Prompt:**
```
this?
```
[Screenshot: the ".NET Install Tool" extension page in VS Code, already installed]

**AI response (full):**
````
Not quite. That's the **.NET Install Tool** extension, and it looks like you've already installed it (it shows "Disable" and "Uninstall"). It's harmless, so you can leave it, but **it isn't the .NET SDK itself**. As its own page says, it's a helper that other extensions use to install .NET.

You still need the SDK from Microsoft's website:

1. Open **[dotnet.microsoft.com/en-us/download/dotnet/10.0](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)** in your web browser (Safari or Chrome, not VS Code).
2. In the **SDK** section, under **macOS**, click **Installer → Arm64** (your MacBook Air almost certainly has an Apple chip; check under Apple menu → About This Mac → "Chip").
3. Open the downloaded `.pkg` and click through the installer.
4. Quit and reopen VS Code, then type this in the terminal at the bottom and press Enter:
```
dotnet --version
```

Send me what it prints.
````

**What I did with it:** Installed the .NET 10 SDK (Arm64) from Microsoft's website.

**Critique (my own words):**

---

### Entry 14 – 2026-11-05 03:49 – Branch: `main` (no code)

**Prompt:**
```
i installed it
```

**AI response (full):**
````
Good. Now check it worked: quit VS Code completely (**Cmd+Q**), reopen it, then type this in the terminal at the bottom and press Enter:

```
dotnet --version
```

Send me the number it prints (or a screenshot).
````

**Critique (my own words):**

---

### Entry 15 – 2026-11-05 03:50 – Branch: `main` – Setup test code

**Prompt:**
```
here
```
[Screenshot: terminal showing `dotnet --version` → 10.0.403]

**AI response (full, including code):**
````
.NET **10.0.403** is installed, so you're ready.

**1. Delete the stray file first.** The empty `StarChart3D` text file would clash with the project folder of the same name. Close its tab in the editor, then run:
```
rm StarChart3D
```

**2. Create the project and add Raylib-cs** (run these one at a time, from the same folder you're in now):
```
dotnet new console -n StarChart3D
cd StarChart3D
dotnet add package Raylib-cs
dotnet build
```
The last command should end with **"Build succeeded"**.

**3. Test that a window opens on your Mac.** Open `StarChart3D/Program.cs`, replace everything in it with this, and save:
```csharp
using Raylib_cs;

Raylib.InitWindow(800, 450, "StarChart3D - setup test");
Raylib.SetTargetFPS(60);

while (!Raylib.WindowShouldClose())
{
    Raylib.BeginDrawing();
    Raylib.ClearBackground(Color.Black);
    Raylib.DrawText("Raylib-cs is working on macOS", 20, 20, 24, Color.White);
    Raylib.EndDrawing();
}

Raylib.CloseWindow();
```
Then run:
```
dotnet run
```
A black window with white text should appear. Close it with **Esc**.

Send me a screenshot of the result, or any error if it fails. Remember this test code came from me, so it goes in your `AI_programming_log.md` too.
````

**What I did with it:** Deleted the stray file, created the console project, added Raylib-cs (version 8.1.0 was installed) and built it successfully. But `dotnet run` printed "Hello, World!" (see next entry).

**Explanation of the code (in my own words):**

**Critique (my own words):**

---

### Entry 16 – 2026-11-05 03:55 – Branch: `main` (no code)

**Prompt:**
```
here
```
[Screenshot: build succeeded with Raylib-cs 8.1.0 and net10.0, but `dotnet run` printed "Hello, World!". The test code was in an unsaved editor tab named "StarChart3D"]

**AI response (full):**
````
Nearly there. The build worked (**Raylib-cs 8.1.0** added, **net10.0 build succeeded**), but `dotnet run` printed **"Hello, World!"**, which means my test code isn't in `Program.cs` yet.

Looking at your screenshot, the code is in the **old "StarChart3D" tab**, the stray file you deleted. It's also **unsaved** (the white dot on the tab). So the project is still running the default `Program.cs`.

To fix it:
1. Copy the code from that tab, then **close the tab** and choose **Don't Save**.
2. In the Explorer on the left, click the arrow next to the **StarChart3D** folder to open it.
3. Open **Program.cs**, delete its contents, paste the code, and **save with Cmd+S**.
4. In the terminal, run:
```
dotnet run
```

You should get a black window with "Raylib-cs is working on macOS". Close it with **Esc**, then send me a screenshot.
````

**Critique (my own words):**

---

### Entry 17 – 2026-11-05 03:58 – Branch: `main` (no code)

**Prompt:**
```
in program .cs, which code to paste ]
```

**AI response (full, including code):**
````
Paste this whole block into `Program.cs`, replacing everything that's there (including the `Console.WriteLine("Hello, World!");` line):

```csharp
using Raylib_cs;

Raylib.InitWindow(800, 450, "StarChart3D - setup test");
Raylib.SetTargetFPS(60);

while (!Raylib.WindowShouldClose())
{
    Raylib.BeginDrawing();
    Raylib.ClearBackground(Color.Black);
    Raylib.DrawText("Raylib-cs is working on macOS", 20, 20, 24, Color.White);
    Raylib.EndDrawing();
}

Raylib.CloseWindow();
```

Then save with **Cmd+S** and run:
```
dotnet run
```
````

**What I did with it:** Pasted it into `Program.cs`, saved, ran it. The window opened (next entry).

**Critique (my own words):**

---

### Entry 18 – 2026-11-05 04:00 – Branch: `main` (no code)

**Prompt:**
```
here
```
[Screenshot: window titled "StarChart3D - setup test" showing "Raylib-cs is working on macOS" in white on black]

**AI response (full):**
````
**Raylib-cs works on your Mac.** That's the Setting Up environment done.

Now let's save this as your first proper commit. These are your steps, as agreed; I won't touch GitHub.

**1. Add the two log files.** Move the `AI_programming_log.md` and `AI_domainExpert_log.md` files I sent you into the `01Assignment1_AIprogramming_3DstarChart` folder (next to the `StarChart3D` folder, not inside it).

**2. Log what we've done so far.** In `AI_programming_log.md`, add entries for this conversation: your prompts and my full replies, especially the Raylib-cs test code. A good point for your **critique** section, in your own words: the tutorial uses WPF, which is Windows-only, so on macOS you chose Raylib-cs instead. Also note the "Hello, World!" problem, where the code was in the wrong, unsaved file.

**3. Check what will be committed.** In the terminal:
```
cd ..
git status
```
You should see only `StarChart3D/` (with `Program.cs` and `StarChart3D.csproj`) and the two `.md` files. **There should be no `bin` or `obj` folders**; your `.gitignore` should already exclude them. If you do see them, stop and send me a screenshot.

**4. Commit and push** (on `main`):
```
git add .
git commit -m "Setup: add .NET 10 Raylib-cs project and AI log files"
git push
```

**5. Create the Feature 1 branch:**
```
git checkout -b feature1-create-star-chart
```

Send me a screenshot after step 3 (`git status`) before committing, so I can double-check what's going in.
````

**Critique (my own words):**

---

### Entry 19 – 2026-11-05 04:02 – Branch: `main` (no code)

**Prompt:**
```
provide me everything in code so i can run through termininal
```

**AI response:** (paste the AI's reply here – it gave me this log file and terminal commands to move the log files into place)

**Critique (my own words):**

---
