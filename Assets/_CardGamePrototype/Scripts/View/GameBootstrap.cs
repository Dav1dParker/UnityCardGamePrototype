using _CardGamePrototype.Scripts.Core;
using _CardGamePrototype.Scripts.View;
using UnityEngine;

namespace _CardGamePrototype.Scripts.View
{
    public sealed class GameBootstrap : MonoBehaviour
    {
        [SerializeField] private StackView deckStack;
        [SerializeField] private StackView[] tableauStacks;
        [SerializeField] private CardView cardPrefab;
        [SerializeField] private int initialDeckSize = 52;

        private void Start()
        {
            for (int i = 0; i < initialDeckSize; i++)
            {
                var card = new Card();
                var view = Instantiate(cardPrefab, deckStack.transform);
                view.Bind(card);
                deckStack.AddCard(view);
            }

            /*
            for (int i = 0; i < tableauStacks.Length; i++)
            {
                var tableau = tableauStacks[i];
                int cardsToMove = Mathf.Min(i + 1, deckStack.transform.childCount);

                for (int j = 0; j < cardsToMove; j++)
                {
                    int lastIndex = deckStack.transform.childCount - 1;
                    if (lastIndex < 0) break;

                    var cardView = deckStack.transform.GetChild(lastIndex).GetComponent<CardView>();
                    cardView.transform.SetParent(tableau.transform, false);
                    tableau.AddCard(cardView);
                }
            }
            */
        }
    }
}