namespace Loudenvier.CommandLineFramework;

public enum CommandCase { 
    /// <summary>Keep the case intact when processing</summary>
    Original, 
    /// <summary>Lowercases all characaters and remove common separators ('_', '-') (ex.: Flat-Case => flatcase)</summary>
    Flat, 
    /// <summary>The beginning char and every first char after a separator is in uppercase (ex.: pascal-case => PascalCase)</summary>
    Pascal, 
    /// <summary>The first char is lowercase and every other first char after a separator is in uppercase 
    /// (ex.: Camel_case => camelCase)</summary>
    Camel, 
    /// <summary>Every uppercase characater is converted to lowercase and the first in a sequence is prepended with '-'
    /// (ex.: KebabCase => kebab-case)</summary>
    Kebab,
    /// <summary>Every uppercase characater is converted to lowercase and the first in a sequence is prepended with '_'
    /// (ex.: SnakeCase => snake_case)</summary>
    Snake, 
    /// <summary>
    /// Uppercases all characters and prepends every original first upper case in a sequence with '_'
    /// (ex.: ConstanCase => CONSTANT_CASE)
    /// </summary>
    Constant }
