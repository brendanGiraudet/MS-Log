namespace ms_log.Constants;

public static class RabbitmqConstants
{
    public const string RecipExchangeName = "recip";

    // RECIP
    public const string CreateRecipResultRoutingKey = "CreateRecipResult";
    
    public const string UpdateRecipResultRoutingKey = "UpdateRecipResult";
    
    public const string DeleteRecipResultRoutingKey = "DeleteRecipResult";
    
    // INGREDIENT
    public const string CreateIngredientResultRoutingKey = "CreateIngredientResult";
    
    public const string UpdateIngredientResultRoutingKey = "UpdateIngredientResult";
    
    public const string DeleteIngredientResultRoutingKey = "DeleteIngredientResult";
}
