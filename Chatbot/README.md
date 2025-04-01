# DialoGPT trained by Nguyen The Lap

## Installing and Importing Libraries

The script installs openai, though it is unnecessary for DialoGPT.
It imports:
 - Torch for handling tensors and model computation.
 - AutoModelForCausalLM and AutoTokenizer from transformers for loading the pre-trained chatbot model.
 - Loading the Chatbot Model

## The load_chatbot() function:

- Loads the microsoft/DialoGPT-medium model.
- Retrieves its tokenizer.
- Moves the model to a GPU if available; otherwise, it remains on the CPU.
- Chat Loop

## The chat() function:

- Calls load_chatbot() to get the tokenizer and model.
- Initializes chat_history_ids to keep track of the conversation.
- User Input Handling

- The loop continues indefinitely, waiting for user input.
- If input is empty, it skips processing.
- Encoding & Generating Responses


> [!NOTE]
> The chatbot sometimes generates responses that seem off-topic or inaccurate.

| Date       | Accuracy (%) | Questions |
|------------|--------------|-----------|
| 01-03-2025 |      0%        |     4      |
| 05-03-2025 |      0%       |     10      |
| 10-03-2025 |      5%        |     20      |
| 12-03-2025 |      5%        |     20      |
| 14-03-2025 |      6%        |     40      |
| 16-03-2025 |      5%        |     40      |
| 18-03-2025 |      5%        |     50      |
| 21-03-2025 |      5.07%        |     50      |
| 24-03-2025 |      5.5%        |     50      |
| 26-03-2025 |      6%        |     50      |
| 27-03-2025 |      6.2%        |     60      |
| 29-03-2025 |      6.1%        |     50      |
| 31-03-2025 |      6%        |     60      |
