if (typeof (Tcc) == "undefined") { Tcc = {} }
if (typeof (Tcc.Account) == "undefined") { Tcc.Account = {} }


Tcc.Account = {
    OnLoad: function (executionContext) {
        var formContext = executionContext.getFormContext();

    },
    CNPJOnChange: function (executionContext) {
        var formContext = executionContext.getFormContext();

        var cnpj = formContext.getAttribute("dyn1_cnpj").getValue();

        if (cnpj != null) {
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
            Tcc.Account.DynamicsAlert("Informe um valor para o CNPJ", "CNPJ incorreto!")
        }
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