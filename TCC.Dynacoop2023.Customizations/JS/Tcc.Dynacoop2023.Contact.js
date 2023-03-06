if (typeof (Tcc) == "undefined") { Tcc = {} }
if (typeof (Tcc.Contact) == "undefined") { Tcc.Contact = {} }


Tcc.Contact = {
    OnLoad: function (executionContext) {
        var formContext = executionContext.getFormContext();

    },
    CPFOnChange: function (executionContext) {
        var formContext = executionContext.getFormContext();

        var cpf = formContext.getAttribute("dyn1_cpf").getValue();

        if (cpf != null && this.ValidaCpf(executionContext) != false) {
            if (cpf.length == 11) {
                var formattedCPF = cpf.replace(/^(\d{3})(\d{3})(\d{3})(\d{2})/, "$1.$2.$3-$4");
                var id = Xrm.Page.data.entity.getId();

                var queryContactId = "";

                if (id.length > 0) {
                    queryContactId += " and contactid ne " + id;
                }

                Xrm.WebApi.online.retrieveMultipleRecords("contact", "?$select=firstname&$filter=dyn1_cpf eq '" + formattedCPF + "'" + queryContactId).then(
                    function success(results) {
                        if (results.entities.length == 0) {
                            formContext.getAttribute("dyn1_cpf").setValue(formattedCPF);
                        } else {
                            formContext.getAttribute("dyn1_cpf").setValue("");
                            Tcc.Contact.DynamicsAlert("O CPF já existe no sistema", "CPF duplicado!")
                        }
                    },
                    function (error) {
                        Tcc.Contact.DynamicsAlert("Por favor contato o administrador", "Erro do sistema")
                    }
                );
            }
            else {
                Tcc.Contact.DynamicsAlert("O CPF digitado está incorreto", "CPF inválido!")
            }
        } else {
            Tcc.Contact.DynamicsAlert("Informe um valor para o CPF", "CPF incorreto!")
        }
    },
    ValidaCpf: function (executionContext) {
        var formContext = executionContext.getFormContext();

        var cpf = formContext.getAttribute("dyn1_cpf").getValue();

        var soma;
        var resto;
        soma = 0;
        if (!cpf ||
            cpf.length != 11 ||
            cpf == "00000000000" ||
            cpf == "11111111111" ||
            cpf == "22222222222" ||
            cpf == "33333333333" ||
            cpf == "44444444444" ||
            cpf == "55555555555" ||
            cpf == "66666666666" ||
            cpf == "77777777777" ||
            cpf == "88888888888" ||
            cpf == "99999999999")
        {
            formContext.getAttribute("dyn1_cpf").setValue(null);
            return false
        }

        for (i = 1; i <= 9; i++) {
            soma = soma + parseInt(cpf.substring(i - 1, i)) * (11 - i);
        }

        resto = soma % 11;

        if (resto == 10 || resto == 11 || resto < 2) {
            resto = 0;
        } else {
            resto = 11 - resto;
        }

        if (resto != parseInt(cpf.substring(9, 10))) {
            formContext.getAttribute("dyn1_cpf").setValue(null);
            return false;
        }

        soma = 0;

        for (i = 1; i <= 10; i++) {
            soma = soma + parseInt(cpf.substring(i - 1, i)) * (12 - i);
        }
        resto = soma % 11;

        if (resto == 10 || resto == 11 || resto < 2) {
            resto = 0;
        } else {
            resto = 11 - resto;
        }

        if (resto != parseInt(cpf.substring(10, 11))) {
            formContext.getAttribute("dyn1_cpf").setValue(null);
            return false;
        }

        return true;

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