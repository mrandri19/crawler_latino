#r @"./FSharp.Data.2.3.2/lib/net40/FSharp.Data.dll"
#r @"./Newtonsoft.Json.6.0.5/lib/net40/Newtonsoft.Json.dll"
let l x = printfn "%A" x

open System
open FSharp.Data

let clean (str: string) =
    let tmp: string = str.Trim().Split([|'('|]).[0].TrimEnd()
    let normal = tmp.Normalize(Text.NormalizationForm.FormD)
    let withoutDiacritics = String.filter (fun c -> Char.GetUnicodeCategory(c) <> Globalization.UnicodeCategory.NonSpacingMark) normal
    withoutDiacritics.ToLower()


type Suggestion = {input: string[]; output: string}
type Word = {name: string; link: string; suggest: Suggestion}

type Words = JsonProvider<"./data/file-1.json">

let f word = System.Diagnostics.Process.Start("/usr/bin/curl", ("-XPUT -F 'word=" + word + "' localhost:8000/words  -H 'Content-Type: multipart/form-data;'"))

// PUT to localhost:8080 all of the collated words
// for i in 0..200 do
//     let words: Words.Root[] = Words.Load (sprintf "./data/file-%d.json" i)
//     let cleanWords = Array.map (fun (word: Words.Root) -> word.Name |> clean) words
//     cleanWords |> Array.toList |> List.map f |> ignore

// Print to STOUT all of the collated words
// for i in 0..1440 do
//         let words: Words.Root[] = Words.Load (sprintf "./data/file-%d.json" i)
//         let cleanWords = Array.map (fun (word: Words.Root) -> word.Name |> clean) words
//         cleanWords |> Array.toList |> List.map (printfn "%s") |> ignore

// Print to STOUT all of the collated words with the links
// let cleanLink (link:string) =
//     link.Split([|'='|]).[1]

// for i in 0..1440 do
//         let words: Words.Root[] = Words.Load (sprintf "./data/file-%d.json" i)
//         let cleanWords = Array.map (fun (word: Words.Root) -> (word.Name |> clean, word.Link |> cleanLink)) words
//         cleanWords |> Array.toList |> List.map (fun (w, s) -> printfn "%s %s" w s) |> ignore

let cleanLink (link:string) =
    link.Split([|'='|]).[1]

for i in 0..20 do
        let words: Words.Root[] = Words.Load (sprintf "./data/file-%d.json" i)
        let cleanWords = Array.map (fun (word: Words.Root) -> (word.Name |> clean, word.Link |> cleanLink)) words
        cleanWords |> Array.toList |> List.map (fun (w, s) -> (sprintf "/%s_%s" w s) |> f ) |> ignore