if (typeof (Tcc) == "undefined") { Tcc = {} }
if (typeof (Tcc.Contact) == "undefined") { Tcc.Contact = {} }


Tcc.Contact = {
    OnLoad: function (executionContext) {
        var formContext = executionContext.getFormContext();

    },
    CPFOnChange: function (executionContext) {
        var formContext = executionContext.getFormContext();

        var cpf = formContext.getAttribute("dyn1_cpf").getValue();

        if (cpf != null) {
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
                Tcc.Contact.DynamicsAlert("O CPF digitado não é valido", "CPF inválido!")
            }
        } else {
            Tcc.Contact.DynamicsAlert("Informe um valor para o CPF", "CPF incorreto!")
        }
    },
    ValidaCpf: function (executionContext)
    {

        ValidaCPF.valida = function () {
            if (typeof this.cpfLimpo === 'undefined') return false;
            if (this.cpfLimpo.length !== 11) return false;
            if (this.isSequencia()) return false;

            const cpfParcial = this.cpfLimpo.slice(0, -2);
            const digito1 = this.criaDigito(cpfParcial);
            const digito2 = this.criaDigito(cpfParcial + digito1);

            const novoCpf = cpfParcial + digito1 + digito2;

            return novoCpf === this.cpfLimpo;
        };

        ValidaCPF.prototype.criaDigito = function (cpfParcial) {
            const cpfArray = Array.from(cpfParcial);

            let regressivo = cpfArray.length + 1;
            const total = cpfArray.reduce((ac, val) => {
                ac += (regressivo * Number(val));
                regressivo--;
                return ac;
            }, 0);

            const digito = 11 - (total % 11);
            return digito > 9 ? '0' : String(digito);
        };

        ValidaCPF.prototype.isSequencia = function () {
            const sequencia = this.cpfLimpo[0].repeat(this.cpfLimpo.length);
            return sequencia === this.cpfLimpo;
        };

        const cpf = new ValidaCPF('705.484.450-52');

        if (cpf.valida()) {
            console.log('Cpf válido');
        }
        else {
            console.log('Cpf inválido');
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