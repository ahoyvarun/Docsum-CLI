#  Docsum-CLI

**Docsum-CLI** is a C# command-line tool for summarizing documents using either a local ML.NET model or OpenAI’s API. It supports translation of summaries and PDF export all while preserving user data privacy.

---

##  Features

-  AI-driven document summarization (local + OpenAI API)
-  Local-only mode for full privacy
-  Translation support (e.g., English → French)
-  Export summaries as PDF
-  Self-improving via feedback-based training

---

## Quick Start
### 1. Clone the repo
```bash
git clone https://github.com/ahoyvarun/Docsum-CLI.git
cd Docsum-CLI
```
### 2. Restore Dependencies
Make sure you have .NET SDK 6.0+ installed.
```bash
dotnet restore
```
### 3. Set up your OpenAI key (if using the API)
```bash
export OPENAI_API_KEY=your-key-here
```
### 4. Run the summarizer
```bash
dotnet run -- --file sample.txt --export --lang de
```

---

## CLI Options
| Flag        | Description |
|-------------|------------------|
| --file | Path to input .txt file  |
| --export | Save the summary as a PDF  |
| --output | Custom name for the output PDF  |
| --lang | Translate summary to a specific language  |
| --label | Help train the model with a document type  |
| --local-only | Force local ML.NET summarization  |
| --no-feedback | Skip interactive feedback step |

---

## Privacy 
By using the --local-only flag, all document processing and ML training occurs entirely on your machine. No cloud access required.


