namespace SuperStrong.Types.Generators;

internal sealed class GeneratorOutput
{
    private readonly StrongTypeModel? _model;
    private readonly ConflictingAttributesInfo? _conflict;
    private readonly NotPartialInfo? _notPartial;

    private GeneratorOutput(
        StrongTypeModel? model,
        ConflictingAttributesInfo? conflict,
        NotPartialInfo? notPartial)
    {
        _model = model;
        _conflict = conflict;
        _notPartial = notPartial;
    }

    public static GeneratorOutput FromModel(StrongTypeModel model)
        => new(model, conflict: null, notPartial: null);

    public static GeneratorOutput FromConflict(ConflictingAttributesInfo conflict)
        => new(model: null, conflict, notPartial: null);

    public static GeneratorOutput FromNotPartial(NotPartialInfo notPartial)
        => new(model: null, conflict: null, notPartial);

    public void Switch(
        Action<StrongTypeModel> onModel,
        Action<ConflictingAttributesInfo> onConflict,
        Action<NotPartialInfo> onNotPartial)
    {
        if (_model is not null)
        {
            onModel(_model);
        }
        else if (_conflict is not null)
        {
            onConflict(_conflict);
        }
        else if (_notPartial is not null)
        {
            onNotPartial(_notPartial);
        }
        else
        {
            throw new InvalidOperationException("Invalid switch state.");
        }
    }
}
