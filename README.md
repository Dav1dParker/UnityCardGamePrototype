\# Card Game Prototype (Unity)



A lightweight Unity prototype demonstrating a modular card stacking and dragging system.

Built with the new Input System, dynamic tableau offsets, and assembly definitions for faster iteration.



\## Features



\### Card Dragging

\- Uses Unity’s new Input System.

\- Drag a single card or a sub-stack.

\- While dragging, cards are half-transparent if the current drop is invalid and fully opaque if valid.



\### Stack System

\- Three stack types:

&nbsp; - Deck — initial pile.

&nbsp; - Tableau — stacked placement with vertical offset.

&nbsp; - Foundation — accepts only one card at a time.

\- Tableau offset \*\*scales automatically\*\* with the number of cards in the stack.



\### Undo System

\- Undo button reverts the last valid move between stacks.



\### Highlights

\- Separation of \*\*view\*\*, \*\*input\*\*, and \*\*core\*\* logic.

\- Assembly Definitions (`.asmdef`) split the project into:

&nbsp; - `\_CardGamePrototype.Core`

&nbsp; - `\_CardGamePrototype.View`

&nbsp; - `\_CardGamePrototype.Input`

&nbsp; - `\_CardGamePrototype.Logic`

&nbsp; - `\_CardGamePrototype.Bootstrap`



\## Getting Started

1\. Open the project in Unity 2022 or newer.

2\. Open `SampleScene`.

3\. Press \*\*Play\*\*. Drag cards between stacks and use the Undo button.





