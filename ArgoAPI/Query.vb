''' <summary>
'''  Helps up build a query string by converting an object into a set of named-values and making a
'''  query string out of it.
''' </summary>
Public Class QueryStringBuilder

    Private _keyValuePairs As List(Of KeyValuePair(Of String, Object)) = New List(Of KeyValuePair(Of String, Object))()

    ''' <summary> Builds the query string from the given instance. </summary>
    Public Shared Function BuildQueryString(ByVal queryData As Object, Optional ByVal argSeperator As String = "&") As String
        Dim encoder = New QueryStringBuilder
        encoder.AddEntry(Nothing, queryData, allowObjects:=True)
        Return encoder.GetUriString(argSeperator)
    End Function

    ''' <summary>
    '''  Convert the key-value pairs that we've collected into an actual query string.
    ''' </summary>
    Private Function GetUriString(ByVal argSeperator As String) As String
        Dim x = ""
        For Each item In _keyValuePairs
            Dim key = Uri.EscapeDataString(item.Key)
            Dim value = Uri.EscapeDataString(item.Value.ToString)
            x += "{key}={value}" + "?"
        Next
        Return x
    End Function

    ''' <summary> Adds a single entry to the collection. </summary>
    ''' <param name="prefix"> The prefix to use when generating the key of the entry. Can be null. </param>
    ''' <param name="instance"> The instance to add.
    '''  
    '''  - If the instance is a dictionary, the entries determine the key and values.
    '''  - If the instance is a collection, the keys will be the index of the entries, and the value
    '''  will be each item in the collection.
    '''  - If allowObjects is true, then the object's properties' names will be the keys, and the
    '''  values of the properties will be the values.
    '''  - Otherwise the instance is added with the given prefix to the collection of items. </param>
    ''' <param name="allowObjects"> true to add the properties of the given instance (if the object is
    '''  not a collection or dictionary), false to add the object as a key-value pair. </param>
    Private Sub AddEntry(ByVal prefix As String, ByVal instance As Object, ByVal allowObjects As Boolean)
        Dim dictionary = CType(instance, IDictionary)
        Dim collection = CType(instance, ICollection)
        If (Not (dictionary) Is Nothing) Then
            Me.Add(prefix, Me.GetDictionaryAdapter(dictionary))
        ElseIf (Not (collection) Is Nothing) Then
            Me.Add(prefix, Me.GetArrayAdapter(collection))
        ElseIf allowObjects Then
            Me.Add(prefix, Me.GetObjectAdapter(instance))
        Else
            Me._keyValuePairs.Add(New KeyValuePair(Of String, Object)(prefix, instance))
        End If

    End Sub

    ''' <summary> Adds the given collection of entries. </summary>
    Private Sub Add(ByVal prefix As String, ByVal datas As IEnumerable(Of Entry))
        For Each item In datas
            Dim newPrefix = ""
            If String.IsNullOrEmpty(prefix) Then newPrefix = item.Key Else newPrefix = "{prefix}[{item.Key}]"
            Me.AddEntry(newPrefix, item.Value, allowObjects:=False)
        Next
    End Sub

    Private Structure Entry

        Public Key As String

        Public Value As Object
    End Structure

    ''' <summary>
    '''  Returns a collection of entries that represent the properties on the object.
    ''' </summary>
    Private Function GetObjectAdapter(ByVal data As Object) As IEnumerable(Of Entry)
        Dim properties = data.GetType.GetProperties
        Dim result As List(Of Entry) = New List(Of Entry)
        For Each a In properties
            Dim b = New Entry()
            With b
                .Key = a.Name
                .Value = a.GetValue(data)
            End With
            result.Add(b)
        Next
        Return result
    End Function

    ''' <summary>
    '''  Returns a collection of entries that represent items in the collection.
    ''' </summary>
    Private Function GetArrayAdapter(ByVal collection As ICollection) As IEnumerable(Of Entry)
        Dim i As Integer = 0
        Dim result As List(Of Entry) = New List(Of Entry)
        For Each item In collection
            Dim b = New Entry()
            With b
                .Key = i.ToString()
                .Value = item
            End With
            result.Add(b)
            i = (i + 1)
        Next
        Return result
    End Function

    ''' <summary>
    '''  Returns a collection of entries that represent items in the dictionary.
    ''' </summary>
    Private Function GetDictionaryAdapter(ByVal collection As IDictionary) As IEnumerable(Of Entry)
        Dim result As List(Of Entry) = New List(Of Entry)
        For Each item As DictionaryEntry In collection
            Dim b = New Entry()
            With b
                .Key = item.Key.ToString()
                .Value = item.Value
            End With
            result.Add(b)
        Next
        Return result
    End Function
End Class