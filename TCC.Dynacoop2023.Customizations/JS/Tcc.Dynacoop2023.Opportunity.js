if (typeof (Tcc) == "undefined") { Tcc = {} }
if (typeof (Tcc.Opportunity) == "undefined") { Tcc.Account = {} }

Tcc.Opportunity = {
    OnSave: function (executionContext) {
        var formContext = executionContext.getFormContext();
        var integracao = formContext.getAttribute("tcc2_integracao").getValue();
        if (integracao) {
            formContext.ui.setFormNotification("Oportunidade bloqueada: Não é possivel alterar a oportunidade pois ela é herdada de uma integração", "WARNING", "oppMsg");
            formContext.data.entity.attributes.forEach(
                function (attribute) {
                    var attributeName = attribute.getName();
                    formContext.getControl(attributeName).setDisabled(true);
                }
            );
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