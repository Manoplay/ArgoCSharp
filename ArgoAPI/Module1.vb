Imports System.IO
Imports System.Net
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq

Public Enum TipoUtente
    Alunno
    Docente
    Genitore
End Enum


Public Class OraLezione
    Public Property numOra As Integer
    Public Property giorno As String
    Public Property prgClasse As Integer
    Public Property prgScuola As Integer
    Public Property lezioni As List(Of Lezioni)
    Public Property numGiorno As Integer
    Public Property codMin As String
End Class

Public Class Lezioni
    Public Property materia As String
    Public Property docente As String
End Class


Public Class Compito
    Public Property datGiorno As String
    Public Property desMateria As String
    Public Property numAnno As Integer
    Public Property prgMateria As Integer
    Public Property prgClasse As Integer
    Public Property desCompiti As String
    Public Property prgScuola As Integer
    Public Property datCompitiPresente As Boolean
    Public Property datCompiti As String
    Public Property docente As String
    Public Property codMin As String
End Class


Public Class Argomento
    Public Property datGiorno As String
    Public Property desMateria As String
    Public Property numAnno As Integer
    Public Property prgMateria As Integer
    Public Property prgClasse As Integer
    Public Property prgScuola As Integer
    Public Property desArgomento As String
    Public Property docente As String
    Public Property codMin As String
End Class


Public Class PrgDocente
    Public Property prgClasse As Integer
    Public Property prgAnagrafe As Integer
    Public Property prgScuola As Integer
    Public Property materie As String
    Public Property docente As Docente
    Public Property codMin As String
End Class

Public Class Docente
    Public Property email As String
    Public Property nome As String
    Public Property cognome As String
End Class


Public Class Voto
    Public Property datGiorno As String
    Public Property desMateria As String
    Public Property prgMateria As Integer
    Public Property prgScuola As Integer
    Public Property prgScheda As Integer
    Public Property codVotoPratico As String
    Public Property decValore As Single
    Public Property codMin As String
    Public Property desProva As String
    Public Property codVoto As String
    Public Property numAnno As Integer
    Public Property prgAlunno As Integer
    Public Property desCommento As String
    Public Property docente As String
End Class


Public Class ElencoAbilitazioni
    Public Property ORARIO_SCOLASTICO As Boolean
    Public Property VALUTAZIONI_PERIODICHE As Boolean
    Public Property COMPITI_ASSEGNATI As Boolean
    Public Property TABELLONE_SCRUTINIO_FINALE As Boolean
    Public Property CURRICULUM_VISUALIZZA_FAMIGLIA As Boolean
    Public Property CONSIGLIO_DI_ISTITUTO As Boolean
    Public Property NOTE_DISCIPLINARI As Boolean
    Public Property ACCESSO_CON_CONTROLLO_SCHEDA As Boolean
    Public Property VOTI_GIUDIZI As Boolean
    Public Property VALUTAZIONI_GIORNALIERE As Boolean
    Public Property IGNORA_OPZIONE_VOTI_DOCENTI As Boolean
    Public Property ARGOMENTI_LEZIONE As Boolean
    Public Property CONSIGLIO_DI_CLASSE As Boolean
    Public Property VALUTAZIONI_SOSPESE_PERIODICHE As Boolean
    Public Property PIN_VOTI As Boolean
    Public Property PAGELLE_ONLINE As Boolean
    Public Property RECUPERO_DEBITO_INT As Boolean
    Public Property RECUPERO_DEBITO_SF As Boolean
    Public Property PROMEMORIA_CLASSE As Boolean
    Public Property VISUALIZZA_BACHECA_PUBBLICA As Boolean
    Public Property CURRICULUM_MODIFICA_FAMIGLIA As Boolean
    Public Property TABELLONE_PERIODI_INTERMEDI As Boolean
    Public Property TASSE_SCOLASTICHE As Boolean
    Public Property DOCENTI_CLASSE As Boolean
    Public Property VISUALIZZA_ASSENZE_REG_PROF As Boolean
    Public Property VISUALIZZA_CURRICULUM As Boolean
    Public Property ASSENZE_PER_DATA As Boolean
    Public Property RICHIESTA_CERTIFICATI As Boolean
    Public Property ACCESSO_SENZA_CONTROLLO As Boolean
    Public Property PRENOTAZIONE_ALUNNI As Boolean
    Public Property MODIFICA_RECAPITI As Boolean
    Public Property PAGELLINO_ONLINE As Boolean
    Public Property MEDIA_PESATA As Boolean
    Public Property GIUSTIFICAZIONI_ASSENZE As Boolean
End Class

Public Class Alunno
    Public Property desCf As String
    Public Property desCognome As String
    Public Property desVia As String
    Public Property desCap As String
    Public Property desNome As String
    Public Property desCellulare As String
    Public Property desComuneNascita As String
    Public Property flgSesso As String
    Public Property datNascita As String
    Public Property desIndirizzoRecapito As String
    Public Property desComuneRecapito As String
    Public Property desCapResidenza As String
    Public Property desComuneResidenza As String
    Public Property desTelefono As Object
    Public Property desCittadinanza As String
End Class

Public Class AnnoScolastico
    Public Property datInizio As String
    Public Property datFine As String
End Class


Public Class Assenza
    Public Property codEvento As String
    Public Property numOra As Object
    Public Property datGiustificazione As String
    Public Property prgScuola As Integer
    Public Property prgScheda As Integer
    Public Property binUid As String
    Public Property codMin As String
    Public Property datAssenza As String
    Public Property numAnno As Integer
    Public Property prgAlunno As Integer
    Public Property flgDaGiustificare As Boolean
    Public Property giustificataDa As String
    Public Property desAssenza As String
    Public Property registrataDa As String
End Class


Public Class Scheda
    Public Property SchedaSelezionata As Boolean = True
    Public Property DescrizioneScuola As String
    Public Property DescrizioneSede As String
    Public Property PrgAlunno As Integer
    Public Property PrgClasse As Integer
    Public Property DesCorso As String
    Public Property DesDenominazione As String
    Public Property Studente As Alunno
    Public Property Abilitazioni As ElencoAbilitazioni
    Public Property AnnoScolastico As AnnoScolastico
    Private Property token As String
    Public Sub New(authToken As String)
        Dim header As Dictionary(Of String, String) = New Dictionary(Of String, String) From {{"x-cod-min", "ss16825"}, {"x-auth-token", authToken}}
        Dim json = Curl("schede", header, New Dictionary(Of String, String)())(0)
        Dim s As JObject
        s = JsonConvert.DeserializeObject(json)(0)
        SchedaSelezionata = s.Value(Of Boolean)("schedaSelezionata")
        DescrizioneScuola = s.Value(Of String)("desScuola")
        DescrizioneSede = s.Value(Of String)("desSede")
        PrgAlunno = s.Value(Of Integer)("prgAlunno")
        PrgClasse = s.Value(Of Integer)("prgClasse")
        DesCorso = s.Value(Of String)("desCorso")
        DesDenominazione = s.Value(Of String)("desDenominazione")
        Studente = s.Item("alunno").ToObject(Of Alunno)
        Abilitazioni = s.Item("abilitazioni").ToObject(Of ElencoAbilitazioni)
        AnnoScolastico = s.Item("annoScolastico").ToObject(Of AnnoScolastico)
        token = authToken
    End Sub

    Public Function VotiGiornalieri() As Voto()
        Dim header As Dictionary(Of String, String) = New Dictionary(Of String, String) From {{"x-cod-min", "ss16825"}, {"x-auth-token", token}, {"x-prg-alunno", PrgAlunno}, {"x-prg-scheda", "1"}, {"x-prg-scuola", "1"}}
        Dim json = Curl("votigiornalieri", header, New Dictionary(Of String, String)())(0)
        Dim j As JObject = JsonConvert.DeserializeObject(json)
        Dim a = j.Item("dati")
        Dim vs As List(Of Voto) = New List(Of Voto)()
        For Each item As JToken In a
            Try
                Dim v As Voto = item.ToObject(Of Voto)
                vs.Add(v)
            Catch
                Debug.WriteLine("ERRORE CON LA LETTURA DI UN VOTO")
            End Try
        Next
        Return vs.ToArray()
    End Function

    Public Function Assenze() As Assenza()
        Dim header As Dictionary(Of String, String) = New Dictionary(Of String, String) From {{"x-cod-min", "ss16825"}, {"x-auth-token", token}, {"x-prg-alunno", PrgAlunno}, {"x-prg-scheda", "1"}, {"x-prg-scuola", "1"}}
        Dim json = Curl("assenze", header, New Dictionary(Of String, String)())(0)
        Dim j As JObject = JsonConvert.DeserializeObject(json)
        Dim a = j.Item("dati")
        Dim vs As List(Of Assenza) = New List(Of Assenza)()
        For Each item As JToken In a
            Try
                Dim v As Assenza = item.ToObject(Of Assenza)
                vs.Add(v)
            Catch
                Debug.WriteLine("ERRORE CON LA LETTURA DI UN'ASSENZA")
            End Try
        Next
        Return vs.ToArray()
        Console.WriteLine(json)
    End Function

    Public Function Docenti() As PrgDocente()
        Dim header As Dictionary(Of String, String) = New Dictionary(Of String, String) From {{"x-cod-min", "ss16825"}, {"x-auth-token", token}, {"x-prg-alunno", PrgAlunno}, {"x-prg-scheda", "1"}, {"x-prg-scuola", "1"}}
        Dim json = Curl("docenticlasse", header, New Dictionary(Of String, String)())(0)
        Dim sar As JArray
        Dim d As List(Of PrgDocente) = New List(Of PrgDocente)()
        sar = JsonConvert.DeserializeObject(json)
        For Each item As JToken In sar
            Dim x As PrgDocente = item.ToObject(Of PrgDocente)
            d.Add(x)
        Next
        Return d.ToArray()
    End Function

    Public Function Compiti() As Compito()
        Dim header As Dictionary(Of String, String) = New Dictionary(Of String, String) From {{"x-cod-min", "ss16825"}, {"x-auth-token", token}, {"x-prg-alunno", PrgAlunno}, {"x-prg-scheda", "1"}, {"x-prg-scuola", "1"}}
        Dim json = Curl("compiti", header, New Dictionary(Of String, String)())(0)
        Dim j As JObject = JsonConvert.DeserializeObject(json)
        Dim a = j.Item("dati")
        Dim d As List(Of Compito) = New List(Of Compito)()
        For Each item As JToken In a
            Dim x As Compito = item.ToObject(Of Compito)
            d.Add(x)
        Next
        Return d.ToArray()
    End Function

    Public Function Argomenti() As Argomento()
        Dim header As Dictionary(Of String, String) = New Dictionary(Of String, String) From {{"x-cod-min", "ss16825"}, {"x-auth-token", token}, {"x-prg-alunno", PrgAlunno}, {"x-prg-scheda", "1"}, {"x-prg-scuola", "1"}}
        Dim json = Curl("argomenti", header, New Dictionary(Of String, String)())(0)
        Dim j As JObject = JsonConvert.DeserializeObject(json)
        Dim a = j.Item("dati")
        Dim d As List(Of Argomento) = New List(Of Argomento)()
        For Each item As JToken In a
            Dim x As Argomento = item.ToObject(Of Argomento)
            d.Add(x)
        Next
        Return d.ToArray()
    End Function

    Public Function Orario() As OraLezione()
        Dim header As Dictionary(Of String, String) = New Dictionary(Of String, String) From {{"x-cod-min", "ss16825"}, {"x-auth-token", token}, {"x-prg-alunno", PrgAlunno}, {"x-prg-scheda", "1"}, {"x-prg-scuola", "1"}}
        Dim json = Curl("orario", header, New Dictionary(Of String, String)())(0)
        Dim j As JObject = JsonConvert.DeserializeObject(json)
        Dim a = j.Item("dati")
        Dim d As List(Of OraLezione) = New List(Of OraLezione)()
        For Each item As JToken In a
            Dim x As OraLezione = item.ToObject(Of OraLezione)
            d.Add(x)
        Next
        Return d.ToArray()
    End Function

End Class

Public Class Sessione
    Public Property Token As String
    Public Property Alunno As TipoUtente
    Public Sub New(scuola As String, nome As String, password As String)
        Dim header As Dictionary(Of String, String) = New Dictionary(Of String, String) From {{"x-cod-min", "ss16825"}, {"x-user-id", "mascolo"}, {"x-pwd", "manoplay"}}
        Dim s As JObject
        s = JsonConvert.DeserializeObject(Curl("login", header, New Dictionary(Of String, String)())(0))
        Token = s.Value(Of String)("token")
        Alunno = TipoUtente.Alunno
    End Sub
End Class

Module ArgoAPI

    Public Const ARGOAPI_URL = "https://www.portaleargo.it/famiglia/api/rest/"
    Public Const ARGOAPI_KEY = "ax6542sdru3217t4eesd9"
    Public Const ARGOAPI_VERSION = "2.0.2"

    Public ReadOnly UnixEpoch As DateTime = New DateTime(1970, 1, 1)
    Public Function GetUnixTimestamp(dt As DateTime) As Long
        Dim span As TimeSpan = dt - UnixEpoch
        Return Math.Round(span.TotalSeconds)
    End Function

    Function QueryS(query As IEnumerable(Of KeyValuePair(Of String, String))) As String
        Dim output As String = ""
        For Each s In query
            output += s.Key + "=" + s.Value + "&"
        Next
    End Function


    ' 	private function curl($request, $auxiliaryHeader, $auxiliaryQuery = array()) {
    Function Curl(request As String, auxiliaryHeader As Dictionary(Of String, String), auxiliaryQuery As Dictionary(Of String, String)) As String()

        Dim defaultHeader As Dictionary(Of String, String) = New Dictionary(Of String, String) From {{"x-key-app", ARGOAPI_KEY}, {"x-version", ARGOAPI_VERSION}}

        Dim header = defaultHeader.Concat(auxiliaryHeader)

        Dim defaultQuery = New Dictionary(Of String, String)
        defaultQuery("_dc") = Math.Round(GetUnixTimestamp(DateTime.UtcNow) * 1000)

        Dim query = defaultQuery.Concat(auxiliaryQuery)

        Dim ch = Net.HttpWebRequest.CreateHttp(ARGOAPI_URL + request + "?" + QueryS(query))

        ch.ServerCertificateValidationCallback = Function()
                                                     Return True
                                                 End Function

        ' curl_setopt($ch, CURLOPT_RETURNTRANSFER, True);

        For Each item In header
            ch.Headers.Add(item.Key, item.Value)
        Next

        ch.UserAgent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/57.0.2987.133 Safari/537.36"

        Dim output = DirectCast(ch.GetResponse(), HttpWebResponse)

        Dim httpcode = DirectCast(output.StatusCode, Integer)

        Dim streamReader As StreamReader = New StreamReader(output.GetResponseStream())

        Dim data = streamReader.ReadToEnd()

        output.Close()

        Return {data, httpcode.ToString()}
    End Function
End Module