using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;
using PayPalHttp;
using System.Globalization;
using System.Net;
using DotNetEnv;

public class PayPalOrderRequestModel
{
    public string ProductId { get; set; } 
    public string ProductName { get; set; } 
    public decimal ProductPrice { get; set; } 
}
public class PayPalCaptureRequestModel
{
    public string OrderId { get; set; } 
}
public class PayPalController : Controller
{
    private readonly PayPalHttpClient _payPalClient;

    public PayPalController(IConfiguration configuration)
    {

        string clientId = System.Environment.GetEnvironmentVariable("CLIENT_ID");
        string clientSecret = System.Environment.GetEnvironmentVariable("CLIENT_SECRET");
        PayPalEnvironment environment = new SandboxEnvironment(clientId, clientSecret); 
        _payPalClient = new PayPalHttpClient(environment);

    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] PayPalOrderRequestModel requestModel)
    {
        try
        {
            var createOrderRequest = new OrdersCreateRequest();
            createOrderRequest.Prefer("return=representation");
            createOrderRequest.RequestBody(BuildRequestBody(requestModel));

            var response = await _payPalClient.Execute(createOrderRequest);

            var order = response.Result<PayPalCheckoutSdk.Orders.Order>();

            return Ok(new { orderId = order.Id, productId = requestModel.ProductId, productName = requestModel.ProductName, productPrice = requestModel.ProductPrice });
        }
        catch (HttpException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> CapturePayment([FromBody] PayPalCaptureRequestModel requestModel)
    {
        try
        {
            var captureOrderRequest = new OrdersCaptureRequest(requestModel.OrderId);
            captureOrderRequest.RequestBody(new OrderActionRequest());

            var response = await _payPalClient.Execute(captureOrderRequest);

            if (response.StatusCode == HttpStatusCode.Created)
            {
                return Ok(new { success = true });
            }

            return BadRequest(new { error = "Не удалось захватить оплату." });
        }
        catch (HttpException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    private OrderRequest BuildRequestBody(PayPalOrderRequestModel requestModel)
    {
        var orderRequest = new OrderRequest()
        {
            CheckoutPaymentIntent = "CAPTURE",
            PurchaseUnits = new List<PurchaseUnitRequest>()
            {
                new PurchaseUnitRequest()
                {
                    ReferenceId = "default",
                    AmountWithBreakdown = new AmountWithBreakdown()
                    {
                        CurrencyCode = "USD",
                        Value = requestModel.ProductPrice.ToString("F2", CultureInfo.InvariantCulture),
                    },
                },
            },
        };

        return orderRequest;
    }
}
