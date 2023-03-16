# Osyare Chrono (おしゃれクロノ)

Oshare Chrono is a clock application that was developed for a UI/UX programming challenge. The requirements was that this clock was to be used at work and can be used by workers to track their working time together with other applications.
I designed the UI, UI layout, the radial graphic and coded it.

## Features:
**Clock**
![Clock View](https://user-images.githubusercontent.com/9075833/225640182-3d851e1d-9be0-4b80-880a-cb8593ba7c1f.png)

- Bauhaus colour palette-inspired radial display design for the clock.
  - Innermost circle represents the hour. The circle changes colours depending on the time of day to represent the day-night cycle. It changes to blue when the time is 6:30 P.M (to represent night) and turns red when the time is 6:30 a.m. (to represent day time).
  - Middle ring represents the minutes.
  - Outer ring represents the seconds.
- 12-hour digital format.
- User's current time zone display.
- Shows the user's current time in their timezone.
- From the inside: Innermost is Hour, middle ring represents the Minute and outer most 

**Countdown Timer**
![Countdown Timer View](https://user-images.githubusercontent.com/9075833/225640667-bca4f855-fe64-453b-b4d5-298545294bac.png)

- Minute-based countdown timer
- Set countdown timer on 1-minute intervals.
- Max countdown time: 59 minutes.
- Min countdown time: 1 minute.

**Stopwatch**
![Stopwatch View](https://user-images.githubusercontent.com/9075833/225640631-ecacec0b-13ab-488e-8ecd-6f11ac0e18f9.png)

- 1/100 stopwatch
- Maximum time is 59:99.999 minutes each .
- Can count for up to 24 hours max.

Packages used:
- Lean GUI
- DOTween

Asset packs used:
- Kenney UI Pack (https://www.kenney.nl/assets/ui-pack)
- flaticon (https://www.flaticon.com/)
