```markdown

\# Card Game Prototype (Unity)



A lightweight Unity prototype demonstrating a modular card stacking and dragging system.  

Built with the new Input System, dynamic tableau offsets, and fully separated assemblies for faster iteration.



---



\## Features



\### Card Dragging

\- Click and drag cards using Unity’s new Input System

\- Supports dragging single cards or entire sub-stacks.

\- Cards become half-transparent if they can't be placed at current point.



\### Stack System

\- Three stack types:

&nbsp; - \*\*Deck\*\* – holds the initial cards.

&nbsp; - \*\*Tableau\*\* – allows stacked placement with offset.

&nbsp; - \*\*Foundation\*\* – accepts only one card at a time.

\- Offset \*\*scales automatically\*\* with the number of cards in the stack, so taller piles have greater spacing.



\### Undo System

\- Undo button reverts the last valid move between stacks.

\- Works with all stack types.



\### Highlights

\- Separation between \*\*view logic\*\*, \*\*input handling\*\*, and \*\*core systems\*\*.

\- Reusable and extendable architecture suitable for solitaire-style or card-management games.

\- Uses \*\*Assembly Definition Files\*\* (`.asmdef`) to split the project into modular assemblies:

---



\## Getting Started

1\. Clone the repository and open the project in Unity 6 or newer.

2\. Press \*\*Play\*\* in `SampleScene`.

3\. Drag cards between stacks and test undo functionality.



---

