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
    OnChangeButton: function (primaryControl) {
        var formContext = primaryControl;
        //var formContext = typeof executionContext.getFormContext === "function" ? executionContext.getFormContext() : executionContext;
        var idOpportunidade = formContext.data.entity.getId().replace("{", "").replace("}", "");

        // Obter o ID da oportunidade original a ser clonada
        var originalOpportunityId = idOpportunidade;

        // Criar uma nova entidade para a nova oportunidade
        var clonedOpportunity = {
            "name": "Nova oportunidade",
            // adicione aqui outros campos que você queira clonar
        };

        // Criar um objeto de parâmetro para a Action personalizada
        var parameters = {};
        parameters["OriginalOpportunityId"] = originalOpportunityId;
        parameters["ClonedOpportunity"] = clonedOpportunity;

        // Chamar a Action personalizada para clonar a oportunidade
        var req = new XMLHttpRequest();
        req.open("POST", Xrm.Page.context.getClientUrl() + "/api/data/v9.1/dyn1_ClonaOpp", true);
        req.setRequestHeader("OData-MaxVersion", "4.0");
        req.setRequestHeader("OData-Version", "4.0");
        req.setRequestHeader("Accept", "application/json");
        req.setRequestHeader("Content-Type", "application/json; charset=utf-8");
        req.onreadystatechange = function () {
            if (this.readyState === 4) {
                req.onreadystatechange = null;
                if (this.status === 204) {
                    // A Action foi chamada com sucesso, você pode executar outras ações aqui se necessário
                } else {
                    // A Action falhou, você pode lidar com o erro aqui
                }
            }
        };
        req.send(JSON.stringify(parameters));

        };

        Xrm.WebApi.online.execute(execute_new_ActionClonaOportunidadee_Request).then(
            function success(response) {
                if (response.ok) {
                    console.log("Success");
                    var alertStrings = { confirmButtonLabel: "Ok", text: "Uma nova oportunidade foi originada a partir do registro atual", title: "Oportunidade clonada" };
                    var alertOptions = { height: 120, width: 260 };
                    Xrm.Navigation.openAlertDialog(alertStrings, alertOptions).then(
                        function (success) {
                            console.log("Alert dialog closed");
                        },
                        function (error) {
                            console.log(error.message);
                        }
                    );
                }
                else {
                    var alertStrings = { confirmButtonLabel: "Ok", text: "Erro ao clonar oportunidade", title: "Algo deu errado" };
                    var alertOptions = { height: 120, width: 260 };
                    Xrm.Navigation.openAlertDialog(alertStrings, alertOptions).then(
                        function (success) {
                            console.log("Alert dialog closed");
                        },
                        function (error) {
                            console.log(error.message);
                        }
                    );
                }
            }
        ).catch(function (error) {
            console.log(error.message);
        });
        alert("Clicou no botão");
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