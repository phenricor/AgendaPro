using AgendaPro.Domain.Entities;
using AgendaPro.Domain.Shared;
using AgendaPro.Infrastructure.Data;
using AgendaPro.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AgendaPro.Tests;

public class CustomerTest
{
    public CustomerTest() { }
    [Fact]
    public void CreateCustomer_WhenValid_ShouldReturnCustomer()
    {
        var customer = Customer.Create(
            "Test",
            "test@test.com",
            "07.922.547/0001-93",
            "41996946063"
        );
        Assert.NotNull(customer);
        Assert.Equal(customer.Value.Email, "test@test.com");
        Assert.Equal(customer.Value.Cnpj.Value, "07.922.547/0001-93");
        Assert.Equal(customer.Value.Name, "Test");
        Assert.Equal(customer.Value.Phone.Value, "11998577826");
    }
    [Fact]
    public void CreateCustomer_WhenCnpjInvalid_ShouldReturnError()
    {
        var customer = Customer.Create(
            "Test",
            "test@test.com",
            "07.922.547/0001-94",
            "11998577826"
        );
        Assert.Equal(customer.Error.Code, CustomerErrors.CnpjInvalid.Code);
    }
    [Fact]
    
    public void CreateCustomer_WhenPhoneInvalid_ShouldReturnError()
    {
        var customer = Customer.Create(
            "Test",
            "test@test.com",
            "07.922.547/0001-93",
            "000000000"
        );
        Assert.Equal(customer.Error.Code, CustomerErrors.PhoneInvalid.Code);
    }

    [Fact]
    public void UpdateCustomer_WhenNoChanges_ShouldUpdateCustomer()
    {
        var customer = Customer.Create(
            "Test",
            "test@test.com",
            "07.922.547/0001-93",
            "11998577826"
        ).Value;
        var updatedCustomer = customer.UpdateProperties(
            "Test",
            "test@test.com",
            "07.922.547/0001-93",
            "11998577826"
        ).Value;
        Assert.NotNull(customer);
        Assert.NotNull(updatedCustomer);
        Assert.Equal(updatedCustomer.Email, customer.Email);
        Assert.Equal(updatedCustomer.Name, customer.Name);
        Assert.Equal(updatedCustomer.Cnpj, customer.Cnpj);
        Assert.Equal(updatedCustomer.Phone, customer.Phone);
    }
    [Fact]
    public void UpdateCustomer_WhenCnpjInvalid_ShouldReturnError()
    {
        var customer = Customer.Create(
            "Test",
            "test@test.com",
            "07.922.547/0001-93",
            "11998577826"
        );
        var updatedCustomer = customer.Value.UpdateProperties(
            "Test",
            "test@test.com",
            "07.922.547/0001-94", // Invalid cnpj
            "11998577826"
        );
        Assert.NotNull(customer);
        Assert.NotNull(updatedCustomer);
        Assert.Equal(updatedCustomer.Error.Code, CustomerErrors.CnpjInvalid.Code);
    }
    [Fact]
    public void UpdateCustomer_WhenPhoneInvalid_ShouldReturnError()
    {
        var customer = Customer.Create(
            "Test",
            "test@test.com",
            "07.922.547/0001-93",
            "11998577826"
        );
        var updatedCustomer = customer.Value.UpdateProperties(
            "Test",
            "test@test.com",
            "07.922.547/0001-93", 
            "000000000"
        );
        Assert.NotNull(customer);
        Assert.NotNull(updatedCustomer);
        Assert.Equal(updatedCustomer.Error.Code, CustomerErrors.PhoneInvalid.Code);
    }

    [Fact]
    public void AddAvailableBlock_WhenValid_ShouldReturnAvailableBlock()
    {
        var customer = Customer.Create(
            "Test",
            "test@test.com",
            "07.922.547/0001-93",
            "11998577826"
        );
        customer.Value.Id = Guid.NewGuid();
        var startDate = DateTime.Today.AddDays(1);
        var endDate = DateTime.Today.AddDays(1).AddMinutes(30);
        var block = customer.Value.AddAvailableBlock(startDate, endDate);
        Assert.NotNull(block.Value);
        Assert.Equal(block.Value.StartDate, startDate);
        Assert.Equal(block.Value.EndDate, endDate);
        Assert.Equal(block.Value.CustomerId, customer.Value.Id);
    }

    [Fact]
    public void AddAvailableBlock_WhenDatesInvalid_ShouldReturnError()
    {
        var customer = Customer.Create(
            "Test",
            "test@test.com",
            "07.922.547/0001-93", 
            "11998577826"
        );
        customer.Value.Id = Guid.NewGuid();
        var startDate = DateTime.Today.AddDays(3);
        var endDate = DateTime.Today.AddDays(2).AddMinutes(30);
        var block = customer.Value.AddAvailableBlock(startDate, endDate);
        Assert.Equal(block.Error.Code, AvailableBlockErrors.StartDateBeforeEndDate.Code);
    }

    [Fact]
    public void AddAvailableBlock_WhenNotAvailable_ShouldReturnFalse()
    {
        var customer = Customer.Create(
            "Test",
            "test@test.com",
            "07.922.547/0001-93",
            "11998577826"
        );
        customer.Value.Id = Guid.NewGuid();
        var startDate = DateTime.Today.AddDays(1);
        var endDate = DateTime.Today.AddDays(1).AddMinutes(30);
        var addAvailableBlock = customer.Value.AddAvailableBlock(startDate, endDate);
        var addAvailableBlockAgain = customer.Value.AddAvailableBlock(startDate, endDate); 
        Assert.Equal(addAvailableBlockAgain.Error.Code, AvailableBlockErrors.CustomerNotAvailable.Code);
    }
}