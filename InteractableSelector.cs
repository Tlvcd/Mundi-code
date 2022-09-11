using System.Collections.Generic;
using UnityEngine;
using Axis.Abstractions;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InteractableSelector : MonoBehaviour
{
	#region cached_vars
	private PlayerInputs _inputs;
	private List<IInteractable> _interactablesNearby;
	#endregion
	[SerializeField] InteractableButton interactUIButton;
    [SerializeField] private ScrollRect scrollContainer;

	[SerializeField] GameObject keyBind;
    private RectTransform scrollRect;

	private List<InteractableButton> buttonPool= new List<InteractableButton>();
	private List<InteractableButton> currentlyUsedButtons = new List<InteractableButton>();

	IInteractable interact;
	InteractableButton button;

	[SerializeField] private PlayerState state;

	private void Awake()
	{
		_inputs = PlayerInputManagerClass.GetInputClass();
        scrollRect = scrollContainer.GetComponent<RectTransform>();
    }

	private void OnEnable()
	{
		PlayerInteraction.GetNearbyInteractables += UpdateInteractables;
        _inputs.UI.select.performed += ScrollSelection;
        _inputs.BasePlayer.Interact.performed += InvokeSelectedButton;

    }

    private void InvokeSelectedButton(InputAction.CallbackContext obj)
    {
        if (currentlyUsedButtons.Count == 0 ||state.IsPerformingAction) return;

		currentlyUsedButtons[currSelectionIndex].TriggerEvent();
    }


    private void OnDisable()
	{
		PlayerInteraction.GetNearbyInteractables -= UpdateInteractables;
        _inputs.UI.select.performed -= ScrollSelection;
        _inputs.BasePlayer.Interact.performed -= InvokeSelectedButton;

	}

    private void ScrollSelection(InputAction.CallbackContext obj)
    {
        var value = obj.ReadValue<float>();
        if (value < 0) currSelectionIndex++;
        else currSelectionIndex--;

		SelectButton();
    }

	private void RefreshInteractableButtons()
	{
		currentlyUsedButtons.Clear();

		int j = 0;//reverse iteration
		 for (int i= _interactablesNearby.Count; i >0; --i)
		{
			interact = _interactablesNearby[i-1];
			if (j > buttonPool.Count-1 )//jezeli nie ma wystarczajaco przyciskow, tworzy nowy
			{
				InteractableButton newButton = Instantiate(interactUIButton, transform);
				newButton.SetButtonLabel(interact.GetName());
				newButton.OnInteractFunction = interact.Interaction;
				buttonPool.Add(newButton);
				currentlyUsedButtons.Add(newButton);
			}
			else //wybiera z listy utworzonych przyciskow
			{
				button = buttonPool[j];
				button.SetButtonLabel(interact.GetName());
				button.OnInteractFunction = interact.Interaction;
				button.gameObject.SetActive(true);
				currentlyUsedButtons.Add(button);
			}
			++j;
		}



		for(int a=0; a< buttonPool.Count; a++)//wylacza nie uzywane przyciski
		{
			if (!currentlyUsedButtons.Contains(buttonPool[a])) { buttonPool[a].gameObject.SetActive(false);}
		}
		
		
		SelectButton();
	}

    private RectTransform oldRect;
    private void ScrollToObject(GameObject obj) //scroll do wybranego obiektu
    {
		
        RectTransform rect = obj.GetComponent<RectTransform>();
		Vector2 v = rect.position; 
		bool inView = RectTransformUtility.RectangleContainsScreenPoint(scrollRect, v);
		
		float incrimentSize = rect.rect.height*2; 
		
		if (!inView) 
		{
		    if (oldRect != null) 
		    {
		        if (oldRect.localPosition.y < rect.localPosition.y) 
		        {
		            scrollContainer.content.anchoredPosition += new Vector2(0, -incrimentSize); 
		        }
		        else if (oldRect.localPosition.y > rect.localPosition.y) 
		        {
                    scrollContainer.content.anchoredPosition += new Vector2(0, incrimentSize); 
		        }
		    }

            oldRect = rect; 
        }
	}


    private int currSelectionIndex=0;
    private void SelectButton()
    {
		EventSystem.current.SetSelectedGameObject(null);

        var currCount = currentlyUsedButtons.Count;
		if (currCount == 0)
		{
			keyBind.SetActive(false);
			return;
		}

		keyBind.SetActive(true);
        currSelectionIndex = Mathf.Clamp(currSelectionIndex, 0, currCount - 1);
        var obj = currentlyUsedButtons[currSelectionIndex].gameObject;

		EventSystem.current.SetSelectedGameObject(obj);

		ScrollToObject(obj);
	}

	private void UpdateInteractables(List<IInteractable> list)
	{
		_interactablesNearby = list;
		RefreshInteractableButtons();
	}

}
