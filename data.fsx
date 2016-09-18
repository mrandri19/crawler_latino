#r @"./FSharp.Data.2.3.2/lib/net40/FSharp.Data.dll"
#r @"./Newtonsoft.Json.6.0.5/lib/net40/Newtonsoft.Json.dll"

open Newtonsoft.Json

open FSharp.Data
open System.Net
open System.IO

type Word = { name: string; link: string}

let rec scrapeAndSave (url:string) (iteration:int32) =
    printfn "Downloading %s, iteration %d" url iteration

    // Dowload the webpage
    let wc = new WebClient()
    wc.Encoding = System.Text.Encoding.UTF8 |> ignore
    let response = wc.DownloadString url

    // Parse the webpage and get the words and the next link
    let data = HtmlDocument.Parse response
    
    let wordsElem = data.CssSelect "a[href^='/dizionario-latino-italiano.php?lemma=']"
    let nextLinkElem::_ = data.CssSelect "a[href^='/dizionario-latino-italiano.php?browse']"

    let extractWordAndLink (elem:HtmlNode) = {name = HtmlNodeExtensions.InnerText elem; link = HtmlAttribute.value (elem.Attribute "href")}
    let words = List.map extractWordAndLink wordsElem

    let nextLink = "http://www.dizionario-latino.com" + HtmlAttribute.value (nextLinkElem.Attribute "href")

    // Serialize to json
    let jsonData = JsonConvert.SerializeObject words

    let fileName = sprintf "file-%d.json" iteration
    use w = new System.IO.StreamWriter(fileName)
    w.Write jsonData
    w.Flush()
    w.Close()

    scrapeAndSave nextLink (iteration+1)




scrapeAndSave "http://www.dizionario-latino.com/dizionario-latino-italiano.php" 0 |> ignore