
public interface IInteractable
{
    public void Interact(PlayerInteract player);
    public void UpdateUI(float progress);
    


}

public interface ITakeDrop
{
    public void TakeDrop(PlayerInteract player);
}


