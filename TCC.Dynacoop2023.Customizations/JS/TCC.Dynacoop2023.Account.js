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
                var id = Xrm.Page.data.entity.getId();

                var queryAccountId = "";

                if (id.length > 0) {
                    queryAccountId += " and accountid ne " + id;
                }

                Xrm.WebApi.online.retrieveMultipleRecords("account", "?$select=name&$filter=dyn1_cnpj eq '" + formattedCNPJ + "'" + queryAccountId).then(
                    function success(results) {
                        if (results.entities.length == 0) {
                            formContext.getAttribute("dyn1_cnpj").setValue(formattedCNPJ);
                        } else {
                            formContext.getAttribute("dyn1_cnpj").setValue("");
                            Tcc.Account.DynamicsAlert("O CNPJ já existe no sistema", "CNPJ duplicado!")
                        }
                    },
                    function (error) {
                        Tcc.Account.DynamicsAlert("Por favor contato o administrador", "Erro do sistema")
                    }
                );
            }
            else {
                Tcc.Account.DynamicsAlert("O CNPJ digitado não é valido", "CNPJ inválido!")
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

        const formContext = typeof executionContext.getFormContext === "function" ? executionContext.getFormContext() : executionContext;
        const cep = formContext.getAttribute("dyn1_cep").getValue();
        var parameters = {};
        parameters.cep = cep;

        var req = new XMLHttpRequest();
        req.open("POST", Xrm.Utility.getGlobalContext().getClientUrl() + "/api/data/v9.2/dyn1_BuscaCEP", true);
        req.setRequestHeader("OData-MaxVersion", "4.0");
        req.setRequestHeader("OData-Version", "4.0");
        req.setRequestHeader("Content-Type", "application/json; charset=utf-8");
        req.setRequestHeader("Accept", "application/json");
        req.onreadystatechange = function () {
            if (this.readyState === 4) {
                req.onreadystatechange = null;
                if (this.status === 200 || this.status === 204) {
                    var result = JSON.parse(this.response);
                    console.log(result);
                    var dadoscep = JSON.parse(result["DadosCep"]);
                    Tcc.Logistics.preencheCamposEndereco(formContext, dadoscep);
                } else {
                    console.log(this.responseText);
                }
            }
        };
        req.send(JSON.stringify(parameters));
    },
    CamposEndereco: function (formContext, dadosCep) {

        formContext.getAttribute('dyn1_logradouro').setValue(dadosCep.logradouro);
        formContext.getAttribute('dyn1_complemento').setValue(dadosCep.localidade);
        formContext.getAttribute('dyn1_uf').setValue(dadosCep.uf);
        formContext.getAttribute('dyn1_bairro').setValue(dadosCep.bairro);
        formContext.getAttribute('dyn1_codigoibge').setValue(dadosCep.ibge);
        formContext.getAttribute('dyn1_ddd').setValue(dadosCep.ddd);

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