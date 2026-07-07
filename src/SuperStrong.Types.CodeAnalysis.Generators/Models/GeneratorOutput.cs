namespace SuperStrong.Types.CodeAnalysis.Generators.Models;

internal sealed class GeneratorOutput
{
    private readonly StrongTypeModel? _model;
    private readonly ConflictingAttributesInfo? _conflict;
    private readonly NotPartialInfo? _notPartial;
    private readonly RecordDeclarationInfo? _record;
    private readonly HasBaseTypeInfo? _hasBaseType;
    private readonly AbstractDeclarationInfo? _abstract;

    private GeneratorOutput(
        StrongTypeModel? model,
        ConflictingAttributesInfo? conflict,
        NotPartialInfo? notPartial,
        RecordDeclarationInfo? record,
        HasBaseTypeInfo? hasBaseType,
        AbstractDeclarationInfo? @abstract)
    {
        _model = model;
        _conflict = conflict;
        _notPartial = notPartial;
        _record = record;
        _hasBaseType = hasBaseType;
        _abstract = @abstract;
    }

    public static GeneratorOutput FromModel(StrongTypeModel model)
        => new(model, conflict: null, notPartial: null, record: null, hasBaseType: null, @abstract: null);

    public static GeneratorOutput FromConflict(ConflictingAttributesInfo conflict)
        => new(model: null, conflict, notPartial: null, record: null, hasBaseType: null, @abstract: null);

    public static GeneratorOutput FromNotPartial(NotPartialInfo notPartial)
        => new(model: null, conflict: null, notPartial, record: null, hasBaseType: null, @abstract: null);

    public static GeneratorOutput FromRecord(RecordDeclarationInfo record)
        => new(model: null, conflict: null, notPartial: null, record, hasBaseType: null, @abstract: null);

    public static GeneratorOutput FromHasBaseType(HasBaseTypeInfo hasBaseType)
        => new(model: null, conflict: null, notPartial: null, record: null, hasBaseType, @abstract: null);

    public static GeneratorOutput FromAbstract(AbstractDeclarationInfo @abstract)
        => new(model: null, conflict: null, notPartial: null, record: null, hasBaseType: null, @abstract);

    public void Switch(
        Action<StrongTypeModel> onModel,
        Action<ConflictingAttributesInfo> onConflict,
        Action<NotPartialInfo> onNotPartial,
        Action<RecordDeclarationInfo> onRecord,
        Action<HasBaseTypeInfo> onHasBaseType,
        Action<AbstractDeclarationInfo> onAbstract)
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
        else if (_abstract is not null)
        {
            onAbstract(_abstract);
        }
        else
        {
            throw new InvalidOperationException("Invalid switch state.");
        }
    }
}
