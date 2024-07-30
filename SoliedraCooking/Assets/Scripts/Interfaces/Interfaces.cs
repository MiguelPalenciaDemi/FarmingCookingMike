
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


