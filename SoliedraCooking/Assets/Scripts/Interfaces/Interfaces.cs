
public interface IInteractable
{
    public void Interact(PlayerInteract player);
    public void UpdateUI(float progress);
    public void ShowUI(bool value);
    
}

public interface ITakeDrop
{
    public void TakeDrop(PlayerInteract player);
}


