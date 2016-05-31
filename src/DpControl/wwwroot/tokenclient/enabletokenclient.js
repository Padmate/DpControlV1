﻿(function ($, swaggerUi) {
    $(function () {
        var settings = {
            authority: 'http://localhost:60/',
            //authority: 'http://developer.chinacloudsites.cn/',
            client_id: 'swagger-implicit',
            popup_redirect_uri: window.location.protocol
                + '//'
                + window.location.host
                + '/tokenclient/popup.html',

            response_type: 'id_token token',
            scope: 'openid profile roles dpcontrolapiscope',

            filter_protocol_claims: true
        },
        manager = new OidcTokenManager(settings),
        $inputApiKey = $('#input_apiKey');


        $inputApiKey.on('dblclick', function () {
            manager.openPopupForTokenAsync()
                .then(function () {
                    $inputApiKey.val(manager.access_token).change();
                }, function (error) {
                    console.error(error);
                });
        });
    });
})(jQuery, window.swaggerUi);