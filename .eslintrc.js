module.exports = {
    "env": {
        "browser": true,
        "es6": true
    },
    "parserOptions": {
        "project": "./tsconfig.json"
    },
    "parser": "@typescript-eslint/parser",
    "plugins": [
        "eslint-plugin-import",
        "@angular-eslint/eslint-plugin",
        "@typescript-eslint"
    ],
    "rules": {
        "@angular-eslint/component-class-suffix": "error",
        "@angular-eslint/component-selector": [
            "error",
            {
                "type": "element",
                "prefix": "",
                "style": "kebab-case"
            }
        ],
        "@angular-eslint/directive-class-suffix": "error",
        "@angular-eslint/directive-selector": [
            "error",
            {
                "type": "attribute",
                "prefix": "",
                "style": "camelCase"
            }
        ],
        "@angular-eslint/no-input-rename": "off",
        "@angular-eslint/no-output-rename": "off",
        "@angular-eslint/use-pipe-transform-interface": "error",
        "@typescript-eslint/adjacent-overload-signatures": "error",
        "@typescript-eslint/array-type": [
            "error",
            {
                "default": "generic"
            }
        ],
        "@typescript-eslint/consistent-type-assertions": "error",
        "@typescript-eslint/consistent-type-definitions": "error",
        "@typescript-eslint/dot-notation": "error",
        "@typescript-eslint/explicit-member-accessibility": [
            "error",
            {
                "accessibility": "explicit",
                "overrides": {
                    "constructors": "off",
                    "accessors": "off"
                }
            }
        ],
        "@typescript-eslint/indent": [
            "error",
            4,
            {
                "VariableDeclarator": "first",
                "ArrayExpression": 1,
                "ObjectExpression": 1,
                "SwitchCase": 1,
                "FunctionDeclaration": {
                    "parameters": "first"
                },
                "FunctionExpression": {
                    "parameters": "off"
                },
                "CallExpression": {
                    "arguments": "off"
                },
                "MemberExpression": "off",
                "ImportDeclaration": 1
            }
        ],
        "@typescript-eslint/member-delimiter-style": [
            "error",
            {
                "multiline": {
                    "delimiter": "semi",
                    "requireLast": true
                },
                "singleline": {
                    "delimiter": "semi",
                    "requireLast": false
                }
            }
        ],
        "@typescript-eslint/member-ordering": [
            "error",
            {
                "default": {
                    "memberTypes": [
                        "public-static-field",
                        "protected-static-field",
                        "private-static-field",
                        "public-static-method",
                        "protected-static-method",
                        "private-static-method",
                        "public-instance-field",
                        "protected-instance-field",
                        "private-instance-field",
                        "public-constructor",
                        "protected-constructor",
                        "private-constructor",
                        "public-instance-method",
                        "protected-instance-method",
                        "private-instance-method"
                    ]
                }
            }
        ],
        "@typescript-eslint/naming-convention": [
            "error",
            {
                "selector": "default",
                "format": [],
                "leadingUnderscore": "forbid",
                "trailingUnderscore": "forbid"
            },
            {
                "selector": "variable",
                "modifiers": [
                    "global",
                    "const"
                ],
                "format": [
                    "camelCase",
                    "UPPER_CASE"
                ]
            },
            {
                "selector": "parameter",
                "format": [
                    "camelCase",
                    "PascalCase"
                ]
            },
            {
                "selector": "interface",
                "format": [
                    "PascalCase"
                ]
            },
            {
                "selector": "class",
                "format": [
                    "PascalCase"
                ]
            },
            {
                "selector": "classProperty",
                "format": [
                    "camelCase",
                    "PascalCase"
                ]
            },
            {
                "selector": "classProperty",
                "modifiers": [
                    "private",
                    "protected"
                ],
                "leadingUnderscore": "require",
                "format": [
                    "camelCase"
                ]
            },
            {
                "selector": "classProperty",
                "modifiers": [
                    "private"
                ],
                "format": [
                    "camelCase"
                ],
                "leadingUnderscore": "require"
            },
            {
                "selector": "classProperty",
                "modifiers": [
                    "static"
                ],
                "format": [
                    "UPPER_CASE"
                ]
            },
            {
                "selector": "classProperty",
                "modifiers": [
                    "static",
                    "private"
                ],
                "format": [
                    "UPPER_CASE"
                ],
                "leadingUnderscore": "require"
            },
            {
                "selector": "accessor",
                "modifiers": [
                    "private"
                ],
                "format": [
                    "camelCase"
                ],
                "leadingUnderscore": "require"
            },
            {
                "selector": "classMethod",
                "leadingUnderscore": "forbid",
                "format": [
                    "camelCase"
                ]
            },
            {
                "selector": "enum",
                "format": [
                    "PascalCase"
                ]
            },
            {
                "selector": "enumMember",
                "format": [
                    "UPPER_CASE",
                    "PascalCase"
                ]
            },
            {
                "selector": "typeParameter",
                "format": [
                    "PascalCase"
                ]
            },
            {
                "selector": "typeAlias",
                "format": [
                    "PascalCase"
                ]
            }
        ],
        "@typescript-eslint/no-empty-function": "error",
        "@typescript-eslint/no-inferrable-types": "off",
        "@typescript-eslint/no-misused-new": "error",
        "@typescript-eslint/no-namespace": "error",
        "@typescript-eslint/no-parameter-properties": "error",
        "@typescript-eslint/no-this-alias": "error",
        "@typescript-eslint/prefer-for-of": "error",
        "@typescript-eslint/quotes": [
            "error",
            "single"
        ],
        "@typescript-eslint/semi": [
            "error",
            "always"
        ],
        "@typescript-eslint/strict-boolean-expressions": "off",
        "@typescript-eslint/triple-slash-reference": [
            "error",
            {
                "path": "always",
                "types": "prefer-import",
                "lib": "always"
            }
        ],
        "@typescript-eslint/type-annotation-spacing": "error",
        "unicode-bom": [
            "error",
            "never"
        ],
        "arrow-body-style": [
            "off"
        ],
        "object-curly-spacing": [
            "error",
            "always",
            {
                "objectsInObjects": true,
                "arraysInObjects": true
            }
        ],
        "arrow-parens": [
            "error",
            "always"
        ],
        "comma-dangle": "error",
        "complexity": [
            "error",
            {
                "max": 20
            }
        ],
        "constructor-super": "error",
        "curly": "error",
        "dot-notation": "error",
        "eqeqeq": [
            "error",
            "always"
        ],
        "guard-for-in": "error",
        "import/no-default-export": "error",
        "import/no-deprecated": "off",
        "indent": [
            "off"
        ],
        "new-parens": "error",
        "no-bitwise": "error",
        "no-caller": "error",
        "no-cond-assign": "error",
        "no-console": [
            "error",
            {
                "allow": [
                    "log",
                    "warn",
                    "dir",
                    "timeLog",
                    "assert",
                    "clear",
                    "count",
                    "countReset",
                    "group",
                    "groupEnd",
                    "table",
                    "dirxml",
                    "error",
                    "groupCollapsed",
                    "Console",
                    "profile",
                    "profileEnd",
                    "timeStamp",
                    "context"
                ]
            }
        ],
        "no-debugger": "error",
        "no-duplicate-imports": "error",
        "no-empty": "error",
        "no-empty-function": "error",
        "no-eval": "error",
        "no-invalid-this": "error",
        "no-multiple-empty-lines": [
            "error",
            {
                "max": 1
            }
        ],
        "no-new-wrappers": "error",
        "no-redeclare": "error",
        "no-shadow": [
            "off"
        ],
        "no-sparse-arrays": "error",
        "no-template-curly-in-string": "error",
        "no-throw-literal": "error",
        "no-undef-init": "error",
        "no-unsafe-finally": "error",
        "no-unused-labels": "error",
        "no-var": "error",
        "no-void": "off",
        "one-var": [
            "error",
            "never"
        ],
        "padding-line-between-statements": [
            "off",
            {
                "blankLine": "always",
                "prev": "*",
                "next": "return"
            }
        ],
        "prefer-const": "error",
        "prefer-template": "error",
        "quotes": [
            "error",
            "single"
        ],
        "radix": "error",
        "semi": "error",
        "use-isnan": "error",
        "no-lonely-if": "error"
    }
};
