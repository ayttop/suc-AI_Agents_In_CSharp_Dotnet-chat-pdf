using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;

using Northwind.Application.AI.Agents;

using OllamaSharp;

namespace Northwind.Infrastructure.AI.Agents.Ollama
{
    internal sealed class OllamaCustomerSupportAgent(
        OllamaAgentFactory agentFactory) : ICustomerSupportAgent
    {
        private readonly AIAgent _agent =
            agentFactory.CreateCustomerSupportAgent();

        public async Task<string> AskAsync(
            string message,
            CancellationToken cancellationToken = default)
        {
            var response = await _agent.RunAsync(
                message,
                cancellationToken: cancellationToken);


            return response.Text;
        }
    }

}
