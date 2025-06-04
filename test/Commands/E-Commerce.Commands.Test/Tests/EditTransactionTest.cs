//using Anis.UnregisteredAccountsTransactions.Commands.Domain.Events;
//using Anis.UnregisteredAccountsTransactions.Commands.Domain.Models.SnapShots;
//using Anis.UnregisteredAccountsTransactions.Commands.Infrastructure.Persistence;
//using Anis.UnregisteredAccountsTransactions.Commands.Presentation.AccessClaimsProto;
//using Anis.UnregisteredAccountsTransactions.Commands.Test.Faker.Events;
//using Anis.UnregisteredAccountsTransactions.Commands.Test.Faker.Requests;
//using Anis.UnregisteredAccountsTransactions.Commands.Test.Protos;
//using Calzolari.Grpc.Net.Client.Validation;
//using E_Commerce.Commands.Test.Helper;
//using Google.Protobuf;
//using Grpc.Core;
//using Microsoft.AspNetCore.Mvc.Testing;
//using Microsoft.Extensions.DependencyInjection;
//using Xunit.Abstractions;

//namespace E_Commerce.Commands.Test.Tests
//{
//    public class EditTransactionTest : IClassFixture<WebApplicationFactory<Program>>
//    {
//        private readonly DatabaseHelper _databaseHelper;

//        private readonly GrpcHelper _grpcHelper;

//        private readonly WebApplicationFactory<Program> _factory;

//        public EditTransactionTest(WebApplicationFactory<Program> factory, ITestOutputHelper helper)
//        {
//            _factory = factory.WithDefaultConfigurations(helper, services =>
//            {
//                services.SetUnitTestsIsolatedEnvironment();
//            });

//            _databaseHelper = new DatabaseHelper(_factory);

//            _grpcHelper = new GrpcHelper(_factory);
//        }

//        [Fact]
//        public async Task VerifyDatabaseConnection()
//        {
//            using var scope = _factory.Services.CreateScope();
//            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

//            var canConnect = await dbContext.Database.CanConnectAsync();
//            Assert.True(canConnect, "Database connection failed.");
//        }

//        [Fact]
//        public async Task EditTransaction_NoHistory_ThrowsNotFoundException()
//        {
//            AccessClaims accessClaims = new AccessClaimsFaker();

//            Metadata headers = new()
//            {
//                {"access-claims-bin", accessClaims.ToByteArray()}
//            };

//            EditTransactionRequest request = new EditTransactionRequestFaker().Generate();

//            RpcException exception = await Assert.ThrowsAsync<RpcException>(() =>
//               _grpcHelper.Send(c => c.EditTransactionAsync(request, headers).ResponseAsync));

//            Assert.Equal(StatusCode.NotFound, exception.StatusCode);
//        }

//        [Fact]
//        public async Task EditTransaction_NotFoundAccessClaims_ThrowsUnauthenticated()
//        {
//            EditTransactionRequest request = new EditTransactionRequestFaker()
//                .Generate();

//            RpcException exception = await Assert.ThrowsAsync<RpcException>(() =>
//                _grpcHelper.Send(c => c.EditTransactionAsync(request).ResponseAsync));

//            Assert.Equal(StatusCode.Unauthenticated, exception.StatusCode);
//        }


//        [Fact]
//        public async Task EditTransaction_NoChanges_NoEventsSaved()
//        {
//            AccessClaims accessClaims = new AccessClaimsFaker();

//            Metadata headers = new()
//            {
//                {"access-claims-bin", accessClaims.ToByteArray()}
//            };

//            RegisterTransactionRequested @event = new RegisterTransactionRequestedFaker().Generate();

//            await _databaseHelper.AddEventAsync(@event);

//            EditTransactionRequest request = new EditTransactionRequestFaker().WithSameData(@event);

//            EditTransactionResponse response = await _grpcHelper.Send(c => c.EditTransactionAsync(request, headers: headers).ResponseAsync);

//            List<Event> events = await _databaseHelper.GetAllAsync(request.TransactionId);

//            Event singleEvent = Assert.Single(events);

//            Assert.IsType<RegisterTransactionRequested>(singleEvent);
//        }

//        [Fact]
//        public async Task EditTransaction_ThereIsAHistory_EditAllFields_SaveTransactionEditedEvent()
//        {
//            AccessClaims accessClaims = new AccessClaimsFaker();

//            Metadata headers = new()
//            {
//                {"access-claims-bin", accessClaims.ToByteArray()}
//            };

//            RegisterTransactionRequested @event = new RegisterTransactionRequestedFaker().Generate();

//            await _databaseHelper.AddEventAsync(@event);

//            EditTransactionRequest request = new EditTransactionRequestFaker().ForAggregate(@event.AggregateId);

//            EditTransactionResponse response = await _grpcHelper.Send(c => c.EditTransactionAsync(request, headers: headers).ResponseAsync);

//            IReadOnlyList<Event> events = await _databaseHelper.GetAllAsync(request.TransactionId);

//            Event specificEvent = events.Single(e => e.Sequence == 2);

//            Snapshot? snapshot = await _databaseHelper.GetLatestSnapshotAsync(request.TransactionId);

//            Assert.Null(snapshot);

//            Assert.Equal(2, events.Count);

//            AssertEquality.OfTransactionEdited(@event: specificEvent,
//                request: request,
//                response: response,
//                userId: accessClaims.User.Id,
//                sequence: specificEvent.Sequence
//                );
//        }

//        [Theory]
//        [InlineData("0948304255", "0948304244")]
//        public async Task EditTransaction_EditJustPhoneNumber_SaveTransactionEditedEvent(
//            string oldPhoneNumber,
//            string newPhoneNumber
//            )
//        {
//            AccessClaims accessClaims = new AccessClaimsFaker();

//            Metadata headers = new()
//            {
//                {"access-claims-bin", accessClaims.ToByteArray()}
//            };

//            RegisterTransactionRequested @event = new RegisterTransactionRequestedFaker()
//                .WithPhoneNumber(oldPhoneNumber)
//                .Generate();

//            await _databaseHelper.AddEventAsync(@event);

//            EditTransactionRequest request = new EditTransactionRequestFaker()
//                .ForAggregate(@event.AggregateId)
//                .RuleFor(x => x.PhoneNumber, newPhoneNumber);

//            EditTransactionResponse response = await _grpcHelper.Send(c => c.EditTransactionAsync(request, headers: headers).ResponseAsync);

//            IReadOnlyList<Event> events = await _databaseHelper.GetAllAsync(request.TransactionId);

//            Event specificEvent = events.Single(e => e.Sequence == 2);

//            Snapshot? snapshot = await _databaseHelper.GetLatestSnapshotAsync(request.TransactionId);

//            Assert.Null(snapshot);

//            Assert.Equal(2, events.Count);

//            AssertEquality.OfTransactionEdited(
//                @event: specificEvent,
//                request: request,
//                response: response,
//                userId: accessClaims.User.Id,
//                sequence: specificEvent.Sequence
//                );
//        }

//        [Theory]
//        [InlineData(1000, 500)]
//        [InlineData(500, 1000)]
//        public async Task EditTransaction_EditJustValue_SaveTransactionEditedEvent(
//          decimal oldValue,
//          double newValue
//          )
//        {
//            AccessClaims accessClaims = new AccessClaimsFaker();

//            Metadata headers = new()
//            {
//                {"access-claims-bin", accessClaims.ToByteArray()}
//            };

//            RegisterTransactionRequested @event = new RegisterTransactionRequestedFaker()
//                .WithValue(oldValue)
//                .Generate();

//            await _databaseHelper.AddEventAsync(@event);

//            EditTransactionRequest request = new EditTransactionRequestFaker()
//                .ForAggregate(@event.AggregateId)
//                .RuleFor(x => x.Value, newValue);

//            EditTransactionResponse response = await _grpcHelper.Send(c => c.EditTransactionAsync(request, headers: headers).ResponseAsync);

//            IReadOnlyList<Event> events = await _databaseHelper.GetAllAsync(request.TransactionId);

//            Event specificEvent = events.Single(e => e.Sequence == 2);

//            Snapshot? snapshot = await _databaseHelper.GetLatestSnapshotAsync(request.TransactionId);

//            Assert.Null(snapshot);

//            Assert.Equal(2, events.Count);

//            AssertEquality.OfTransactionEdited(@event: specificEvent,
//                request: request,
//                response: response,
//                userId: accessClaims.User.Id,
//                sequence: specificEvent.Sequence
//                );
//        }

//        [Theory]
//        [InlineData(500, "0911111111", "First edit details")]
//        [InlineData(1000, "0955555555", "Alt edit 1")]
//        public async Task EditTransaction_EditFieldsTwice_SavesBothEditedEvents(
//            decimal secondNewValue,
//            string secondPhoneNumber,
//            string secondDetails)
//        {

//            AccessClaims accessClaims = new AccessClaimsFaker();

//            Metadata headers = new()
//            {
//                { "access-claims-bin", accessClaims.ToByteArray() }
//            };


//            RegisterTransactionRequested @event = new RegisterTransactionRequestedFaker().Generate();

//            await _databaseHelper.AddEventAsync(@event);

//            TransactionEdited editEvent = new TransactionEditedFaker()
//                .WithAggregateId(@event.AggregateId)
//                .WithSequence(@event.Sequence + 1)
//                .Generate();

//            await _databaseHelper.AddEventAsync(editEvent);

//            EditTransactionRequest request = new EditTransactionRequestFaker()
//                .ForAggregate(@event.AggregateId)
//                .WithData(secondPhoneNumber, secondNewValue, secondDetails);

//            EditTransactionResponse response = await _grpcHelper.Send(
//                c => c.EditTransactionAsync(request, headers: headers).ResponseAsync);

//            IReadOnlyList<Event> events = await _databaseHelper.GetAllAsync(@event.AggregateId);

//            Event specificEvent = events.Single(e => e.Sequence == 3);

//            Snapshot? snapshot = await _databaseHelper.GetLatestSnapshotAsync(@event.AggregateId);

//            Assert.NotNull(snapshot);

//            Assert.Equal(3, events.Count);

//            AssertEquality.OfTransactionEditedTwice(
//                events: specificEvent,
//                request: request,
//                response: response,
//                accessClaims.User.Id,
//                sequence: specificEvent.Sequence
//                );
//        }



//        [Theory]
//        [InlineData("hello", " ")]
//        [InlineData("hello", "")]
//        [InlineData(" ", "hello")]
//        [InlineData("", "hello")]
//        public async Task EditTransaction_EditJustDetails_SaveTransactionEditedEvent(
//          string oldDetails,
//          string newDetails
//        )
//        {
//            AccessClaims accessClaims = new AccessClaimsFaker();

//            Metadata headers = new()
//            {
//                {"access-claims-bin", accessClaims.ToByteArray()}
//            };

//            RegisterTransactionRequested @event = new RegisterTransactionRequestedFaker()
//                .WithDetails(oldDetails)
//                .Generate();

//            await _databaseHelper.AddEventAsync(@event);

//            EditTransactionRequest request = new EditTransactionRequestFaker()
//                .ForAggregate(@event.AggregateId)
//                .RuleFor(x => x.Details, newDetails);

//            EditTransactionResponse response = await _grpcHelper.Send(c => c.EditTransactionAsync(request, headers: headers).ResponseAsync);

//            IReadOnlyList<Event> events = await _databaseHelper.GetAllAsync(request.TransactionId);

//            Event specificEvent = events.Single(e => e.Sequence == 2);

//            Snapshot? snapshot = await _databaseHelper.GetLatestSnapshotAsync(request.TransactionId);

//            Assert.Null(snapshot);

//            Assert.Equal(2, events.Count);

//            AssertEquality.OfTransactionEdited(@event: specificEvent,
//                request: request,
//                response: response,
//                userId: accessClaims.User.Id,
//                sequence: specificEvent.Sequence
//                );
//        }

//        [Theory]
//        [InlineData("123456789", true, 100, nameof(EditTransactionRequest.PhoneNumber))]
//        [InlineData("0934567", true, 150, nameof(EditTransactionRequest.PhoneNumber))]
//        [InlineData("093456789012", true, 150, nameof(EditTransactionRequest.PhoneNumber))]
//        [InlineData("08912345678", true, 200, nameof(EditTransactionRequest.PhoneNumber))]
//        [InlineData("09312345678", true, 200, nameof(EditTransactionRequest.PhoneNumber))]
//        [InlineData("+21708912345678", true, 1, nameof(EditTransactionRequest.PhoneNumber))]
//        [InlineData("218093123456", true, 10, nameof(EditTransactionRequest.PhoneNumber))]
//        [InlineData("abcde09312345678", true, 20, nameof(EditTransactionRequest.PhoneNumber))]
//        [InlineData("093 123 4567", true, 30, nameof(EditTransactionRequest.PhoneNumber))]
//        [InlineData("093-123-4567", true, 40, nameof(EditTransactionRequest.PhoneNumber))]
//        [InlineData("+", true, 50, nameof(EditTransactionRequest.PhoneNumber))]
//        [InlineData("+2180931234567", true, 60, nameof(EditTransactionRequest.PhoneNumber))]
//        [InlineData("002180931234567", true, 70, nameof(EditTransactionRequest.PhoneNumber))]
//        [InlineData("+963931234567", true, 80, nameof(EditTransactionRequest.PhoneNumber))]
//        [InlineData("21809123456", true, 90, nameof(EditTransactionRequest.PhoneNumber))]
//        [InlineData("21809123456789", true, 100, nameof(EditTransactionRequest.PhoneNumber))]
//        [InlineData(" ", true, 1100, nameof(EditTransactionRequest.PhoneNumber))]
//        [InlineData("", true, 1120, nameof(EditTransactionRequest.PhoneNumber))]
//        [InlineData("0912304255", false, 2000, nameof(EditTransactionRequest.Details))]
//        [InlineData("0912304255", true, -1, nameof(EditTransactionRequest.Value))]
//        [InlineData("0912304255", true, 0, nameof(EditTransactionRequest.Value))]
//        public async Task EditTransaction_InvalidData_ThrowsInvalidArgument(
//           string phoneNumber,
//           bool isDetailsValid,
//           double value,
//           string errorPropertyName
//        )
//        {
//            EditTransactionRequest request = new EditTransactionRequestFaker()
//                .RuleFor(x => x.PhoneNumber, f => phoneNumber)
//                .RuleFor(x => x.Value, f => value)
//                .Generate();

//            if (!isDetailsValid)
//                request.Details = new string('a', 501);

//            RpcException exception = await Assert.ThrowsAsync<RpcException>
//               (() => _grpcHelper.Send(x => x.EditTransactionAsync(request)).ResponseAsync);

//            Assert.Equal(StatusCode.InvalidArgument, exception.StatusCode);

//            Assert.Contains(
//              exception.GetValidationErrors(),
//              e => e.PropertyName.EndsWith(errorPropertyName)
//            );
//        }

//    }
//}
