/**
 * Obtiene la puntuación media de una oportunidad ejecutando una consulta y sacando el promedio
 * @param {any} executionContext
 */
this.formOnLoad = function (executionContext) {
    var context = executionContext.getFormContext();
    var id = context.data.entity.getId();

    // Se arma el query a ejecutar relacionando los productos asociados a una oportunidad y
    // calculando la media de los productos.
    var query = `
    <fetch version='1.0' output-format='xml-platform' mapping='logical' distinct='false' aggregate='true'>
        <entity name='product'>
            <attribute name='evd_puntuacion' alias='puntuacionmedia' aggregate='avg' />
            <link-entity name='opportunityproduct' from='productid' to='productid' alias='opportunityline' link-type='inner'>
                <link-entity name='opportunity' from='opportunityid' to='opportunityid' alias='opportunity' link-type='inner'>
                    <filter type='and'>
                        <condition attribute='opportunityid' operator='eq' value='${id}' />
                    </filter>
                </link-entity>
            </link-entity>
        </entity>
    </fetch >`;
    query = "?fetchXml=" + this.encodeURIComponent(query);

    Xrm.WebApi.retrieveMultipleRecords('product', query).then(
        function success(result) {
            var puntuacionMedia = result.entities[0].puntuacionmedia;
            context.getAttribute("evd_puntuacionmedia").setValue(puntuacionMedia);
        }
    );
};