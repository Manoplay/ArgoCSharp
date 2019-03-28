Imports System.Text

Module Module1

    Property sessione As Argo.Sessione
    Property scheda As Argo.Scheda


    Function CompletionReadLine() As String
        Dim words() = New String() {"anagrafica", "docenti", "compiti", "argomenti", "voti"}

        Dim sb = New StringBuilder()
        While True
            Dim cki = Console.ReadKey(True)
            If cki.Key = ConsoleKey.Tab Then
                Dim chances = Array.FindAll(words, Function(s)
                                                       If s.Contains(sb.ToString()) Then
                                                           Return True
                                                       End If
                                                       Return False
                                                   End Function)
                If chances.Count = 1 Then
                    sb.Clear()
                    sb.Append(chances.First())
                    Console.CursorLeft = 6
                    Console.Write(chances.First())
                End If
            ElseIf cki.Key = ConsoleKey.Enter Then
                Console.WriteLine()
                Return sb.ToString()
            Else
                Console.Write(cki.KeyChar)
                sb.Append(cki.KeyChar.ToString())
            End If
        End While
    End Function

    Sub Main()
        Console.Write("Scuola: ")
        Dim scuola As String = Console.ReadLine()
        Console.Write("Username: ")
        Dim username As String = Console.ReadLine()
        Console.Write("Password: ")
        Dim password As String = Console.ReadLine()
        sessione = New Argo.Sessione(scuola, username, password)
        scheda = New Argo.Scheda(sessione.Token)
        While True
            Console.Write("ARGO> ")
            Dim comando As String = CompletionReadLine()
            Select Case comando
                Case "anagrafica"
                    Console.WriteLine(scheda.Studente.desNome + " " + scheda.Studente.desCognome)
                    Console.WriteLine(scheda.Studente.desCf)
                    Console.WriteLine("Cittadinanza: " + scheda.Studente.desCittadinanza)
                    Console.WriteLine("Comune di nascita: " + scheda.Studente.desComuneNascita)
                    Console.WriteLine("Comune di residenza: " + scheda.Studente.desComuneRecapito)
                    Console.WriteLine("Indirizzo di recapito: " + scheda.Studente.desIndirizzoRecapito)
                    Console.WriteLine("Numero di telefono: " + scheda.Studente.desTelefono)
                    Console.WriteLine("Scuola: " + scheda.DescrizioneScuola)
                Case "docenti"
                    For Each docente In scheda.Docenti()
                        Console.WriteLine(docente.docente.nome + " " + docente.docente.cognome + " " + docente.materie)
                    Next
                Case "compiti"
                    For Each compito In scheda.Compiti()
                        Console.WriteLine(compito.desMateria + ": " + compito.desCompiti + " " + compito.docente)
                    Next
            End Select
        End While
    End Sub

End Module
