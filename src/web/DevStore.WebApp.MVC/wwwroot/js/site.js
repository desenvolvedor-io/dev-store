function BuscaCep() {
    $(document).ready(function () {

        function limpa_formulário_ZipCode() {
            // Limpa valores do formulário de ZipCode.
            $("#Address_StreetAddress").val("");
            $("#Address_Neighborhood").val("");
            $("#Address_City").val("");
            $("#Address_State").val("");
        }

        //Quando o campo ZipCode perde o foco.
        $("#Address_ZipCode").blur(function () {

            //Nova variável "ZipCode" somente com dígitos.
            var ZipCode = $(this).val().replace(/\D/g, '');

            //Verifica se campo ZipCode possui valor informado.
            if (ZipCode != "") {

                //Expressão regular para validar o ZipCode.
                var validaZipCode = /^[0-9]{8}$/;

                //Valida o formato do ZipCode.
                if (validaZipCode.test(ZipCode)) {

                    //Preenche os campos com "..." enquanto consulta webservice.
                    $("#Address_StreetAddress").val("...");
                    $("#Address_Neighborhood").val("...");
                    $("#Address_City").val("...");
                    $("#Address_State").val("...");

                    //Consulta o webservice viaZipCode.com.br/
                    $.getJSON("https://viacep.com.br/ws/" + ZipCode + "/json/?callback=?",
                        function (dados) {

                            if (!("erro" in dados)) {
                                //Atualiza os campos com os valores da consulta.
                                $("#Address_StreetAddress").val(dados.logradouro);
                                $("#Address_Neighborhood").val(dados.bairro);
                                $("#Address_City").val(dados.localidade);
                                $("#Address_State").val(dados.uf);
                            } //end if.
                            else {
                                //ZipCode pesquisado não foi encontrado.
                                limpa_formulário_ZipCode();
                                alert("Zip Code not found.");
                            }
                        });
                } //end if.
                else {
                    //ZipCode é inválido.
                    limpa_formulário_ZipCode();
                    alert("Invalid Zip Code.");
                }
            } //end if.
            else {
                //ZipCode sem valor, limpa formulário.
                limpa_formulário_ZipCode();
            }
        });
    });
}


function CartaoCredito() {


    if ($("#card_number").length > 0) {
        $("#card_number").validateCreditCard(function (e) {

            $(this).removeClass();
            $(this).addClass("form-control");

            if (e.card_type !== null)
                $(this).addClass(e.card_type.name);

            if (e.valid)
                $(this).addClass("valid");
            else
                $(this).removeClass("valid");

            if (e.length_valid && !e.valid) {
                $(this).addClass("invalid");
            } else {
                $(this).removeClass("invalid");
            }
        });
    }
}