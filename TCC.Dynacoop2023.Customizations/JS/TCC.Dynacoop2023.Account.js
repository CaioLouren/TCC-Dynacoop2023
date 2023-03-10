if (typeof (Tcc) == "undefined") { Tcc = {} }
if (typeof (Tcc.Account) == "undefined") { Tcc.Account = {} }


Tcc.Account = {
    OnLoad: function (executionContext) {
        var formContext = executionContext.getFormContext();

    },
    CNPJOnChange: function (executionContext) {
        var formContext = executionContext.getFormContext();

        var cnpj = formContext.getAttribute("dyn1_cnpj").getValue();

        if (cnpj != null && this.ValidaCnpj(executionContext) != false) {
            if (cnpj.length == 14) {
                var formattedCNPJ = cnpj.replace(/^(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})/, "$1.$2.$3/$4-$5");
                formContext.getAttribute("dyn1_cnpj").setValue(formattedCNPJ);
            }
            else {
                Tcc.Account.DynamicsAlert("O CNPJ digitado não é valido", "CNPJ inválido!")
                formContext.getAttribute("dyn1_cnpj").setValue(null);
            }
        } else {
            Tcc.Account.DynamicsAlert("Informe um CNPJ válido", "CNPJ incorreto!")
        }
    },
    NameOnChange: function (executionContext) {
        var formContext = executionContext.getFormContext();

        var name = formContext.getAttribute("name").getValue();
        var formattedName = name.toLowerCase().replace(/(?:^|\s)(?!da|de|do)\S/g, l => l.toUpperCase());;

        if (name != null) {
            formContext.getAttribute("name").setValue(formattedName);
        }

    },
    ValidaCnpj: function (executionContext) {

        var formContext = executionContext.getFormContext();

        var cnpj = formContext.getAttribute("dyn1_cnpj").getValue();

        if (cnpj == "00000000000000" ||
            cnpj == "11111111111111" ||
            cnpj == "22222222222222" ||
            cnpj == "33333333333333" ||
            cnpj == "44444444444444" ||
            cnpj == "55555555555555" ||
            cnpj == "66666666666666" ||
            cnpj == "77777777777777" ||
            cnpj == "88888888888888" ||
            cnpj == "99999999999999") {
            formContext.getAttribute("dyn1_cnpj").setValue(null);
            return false;
        }

        tamanho = cnpj.length - 2
        numeros = cnpj.substring(0, tamanho);
        digitos = cnpj.substring(tamanho);
        soma = 0;
        pos = tamanho - 7;
        for (i = tamanho; i >= 1; i--) {
            soma += numeros.charAt(tamanho - i) * pos--;
            if (pos < 2)
                pos = 9;
        }
        resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
        if (resultado != digitos.charAt(0)) {
            formContext.getAttribute("dyn1_cnpj").setValue(null);
            return false;
        }

        tamanho = tamanho + 1;
        numeros = cnpj.substring(0, tamanho);
        soma = 0;
        pos = tamanho - 7;
        for (i = tamanho; i >= 1; i--) {
            soma += numeros.charAt(tamanho - i) * pos--;
            if (pos < 2)
                pos = 9;
        }
        resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
        if (resultado != digitos.charAt(1)) {
            formContext.getAttribute("dyn1_cnpj").setValue(null);
            return false;
        }
        return true;
    },
    OnChangeCEP: function (executionContext) {
        var formContext = executionContext.getFormContext();
        var cep = formContext.getAttribute("dyn1_cep").getValue();
        var id = formContext.data.entity.getId();

        var execute_dyn1_BuscaCEP_Request = {
            // Parameters
            entity: { entityType: "account", id: id }, // entity
            Cep: cep,

            getMetadata: function () {
                return {
                    boundParameter: "entity",
                    parameterTypes: {
                        entity: { typeName: "mscrm.account", structuralProperty: 5 },
                        Cep: { typeName: "Edm.String", structuralProperty: 1 }
                    },
                    operationType: 0, operationName: "dyn1_BuscaCEP"
                };
            }
        };

        Xrm.WebApi.online.execute(execute_dyn1_BuscaCEP_Request).then(
            function success(response) {
                if (response.ok) { return response.json(); }
            }
        ).then(function (responseBody) {
            var result = responseBody;
            console.log(result);

            var logradouro = result["logradouro"];
            formContext.getAttribute('dyn1_logradouro').setValue(logradouro);

            var complemento = result["complemento"];
            formContext.getAttribute('dyn1_complemento').setValue(complemento);

            var uf = result["uf"];
            formContext.getAttribute('dyn1_uf').setValue(uf);

            var bairro = result["bairro"];
            formContext.getAttribute('dyn1_bairro').setValue(bairro);

            var ibge = result["ibge"];
            formContext.getAttribute('dyn1_codigoibge').setValue(ibge);

            var ddd = result["ddd"];
            formContext.getAttribute('dyn1_ddd').setValue(ddd);

            var localidade = result["localidade"];
            formContext.getAttribute('dyn1_localidade').setValue(localidade);

            formContext.data.save();
    }).catch(function (error) {
        console.log(error.message);
    });
},
    DynamicsAlert: function (alertText, alertTitle) {
        var alertStrings = {
            confirmButtonLabel: "OK",
            text: alertText,
            title: alertTitle
        };

        var alertOptions = {
            height: 120,
            width: 200
        };

        Xrm.Navigation.openAlertDialog(alertStrings, alertOptions);
    }
}