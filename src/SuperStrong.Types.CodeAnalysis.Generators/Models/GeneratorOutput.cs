namespace SuperStrong.Types.CodeAnalysis.Generators.Models;

internal sealed class GeneratorOutput
{
    private readonly StrongTypeModel? _model;
    private readonly ConflictingAttributesInfo? _conflict;
    private readonly NotPartialInfo? _notPartial;
    private readonly RecordDeclarationInfo? _record;
    private readonly HasBaseTypeInfo? _hasBaseType;

    private GeneratorOutput(
        StrongTypeModel? model,
        ConflictingAttributesInfo? conflict,
        NotPartialInfo? notPartial,
        RecordDeclarationInfo? record,
        HasBaseTypeInfo? hasBaseType)
    {
        _model = model;
        _conflict = conflict;
        _notPartial = notPartial;
        _record = record;
        _hasBaseType = hasBaseType;
    }

    public static GeneratorOutput FromModel(StrongTypeModel model)
        => new(model, conflict: null, notPartial: null, record: null, hasBaseType: null);

    public static GeneratorOutput FromConflict(ConflictingAttributesInfo conflict)
        => new(model: null, conflict, notPartial: null, record: null, hasBaseType: null);

    public static GeneratorOutput FromNotPartial(NotPartialInfo notPartial)
        => new(model: null, conflict: null, notPartial, record: null, hasBaseType: null);

    public static GeneratorOutput FromRecord(RecordDeclarationInfo record)
        => new(model: null, conflict: null, notPartial: null, record, hasBaseType: null);

    public static GeneratorOutput FromHasBaseType(HasBaseTypeInfo hasBaseType)
        => new(model: null, conflict: null, notPartial: null, record: null, hasBaseType);

    public void Switch(
        Action<StrongTypeModel> onModel,
        Action<ConflictingAttributesInfo> onConflict,
        Action<NotPartialInfo> onNotPartial,
        Action<RecordDeclarationInfo> onRecord,
        Action<HasBaseTypeInfo> onHasBaseType)
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
        else if (_record is not null)
        {
            onRecord(_record);
        }
        else if (_hasBaseType is not null)
        {
            onHasBaseType(_hasBaseType);
        }
        else
        {
            throw new InvalidOperationException("Invalid switch state.");
        }
    }
}
