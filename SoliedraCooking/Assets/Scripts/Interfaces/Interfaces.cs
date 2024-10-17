

public interface IInteractable
{
    public void Interact(PlayerInteract player);
    public bool CanInteract();
    public void UpdateUI(float progress);
    


}

public interface ITakeDrop
{
    public void TakeDrop(PlayerInteract player);
    public bool CanTakeDrop();
}

public interface INavigableUI
{
    public void Select(bool value); //Seleccionará visualmente el objeto
    public void Interact(int value = 0); // slider => cambio de valor

    public void Press();
}


