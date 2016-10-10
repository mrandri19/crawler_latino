#r @"./FSharp.Data.2.3.2/lib/net40/FSharp.Data.dll"
#r @"./Newtonsoft.Json.6.0.5/lib/net40/Newtonsoft.Json.dll"
#r @"../trie/lib.dll"
let l x = printfn "%A" x

open System
open FSharp.Data
open AndreaCognolato

let clean (str: string) =
    let tmp: string = str.Trim().Split([|'('|]).[0].TrimEnd()
    let normal = tmp.Normalize(Text.NormalizationForm.FormD)
    let withoutDiacritics = String.filter (fun c -> Char.GetUnicodeCategory(c) <> Globalization.UnicodeCategory.NonSpacingMark) normal
    withoutDiacritics.ToLower()

let trie = Trie.create "a"

type Suggestion = {input: string[]; output: string}
type Word = {name: string; link: string; suggest: Suggestion}

type Words = JsonProvider<"./data/file-1.json">

let f word = System.Diagnostics.Process.Start("/usr/bin/curl", ("-XPUT -F 'word=" + word + "' localhost:8000/words  -H 'Content-Type: multipart/form-data;'"))

for i in 0..200 do
    let words: Words.Root[] = Words.Load (sprintf "./data/file-%d.json" i)
    let cleanWords = Array.map (fun (word: Words.Root) -> word.Name |> clean) words
    cleanWords |> Array.toList |> List.map f
