﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using NUnit.Framework;

namespace DevTrends.WCFDataAnnotations.UnitTests
{
    [TestFixture]
    public class ErrorMessageGeneratorTests
    {
        private const string OperationName = "DoSomething";

        private ErrorMessageGenerator _errorMessageGenerator;

        [SetUp]
        public void Setup()
        {
            _errorMessageGenerator = new ErrorMessageGenerator();
        }

        [Test]
        public void GenerateErrorMessage_Must_Be_Supplied_With_OperationName()
        {
            Assert.Throws<ArgumentNullException>(() => _errorMessageGenerator.GenerateErrorMessage(null, new[] { new ValidationResult("test") }));
        }

        [Test]
        public void GenerateErrorMessage_Must_Be_Supplied_With_ValidationResults()
        {
            Assert.Throws<ArgumentNullException>(() => _errorMessageGenerator.GenerateErrorMessage(OperationName, null));
        }

        [Test]
        public void GenerateErrorMessage_ValidationResults_Cannot_Be_Empty()
        {
            Assert.Throws<ArgumentException>(() => _errorMessageGenerator.GenerateErrorMessage(OperationName, Enumerable.Empty<ValidationResult>()));
        }

        [Test]
        public void GenerateErrorMessage_Result_Contains_OperationName()
        {
            var result = _errorMessageGenerator.GenerateErrorMessage(OperationName, new[] { new ValidationResult("test") });

            Assert.That(result, Is.StringContaining(OperationName));
        }

        [Test]
        public void GenerateErrorMessage_Result_Contains_ValidationResult_ErrorMessage_When_Single_ValidationResult()
        {
            var validationResult = new ValidationResult("oops error");

            var result = _errorMessageGenerator.GenerateErrorMessage(OperationName, new[] { validationResult });

            Assert.That(result, Is.StringContaining(validationResult.ErrorMessage));
        }

        [Test]
        public void GenerateErrorMessage_Result_Contains_ValidationResults_ErrorMessages_When_Multiple_ValidationResults()
        {
            var validationResult = new ValidationResult("oops error");
            var validationResult2 = new ValidationResult("another problem");

            var result = _errorMessageGenerator.GenerateErrorMessage(OperationName, new[] { validationResult, validationResult2 });

            Assert.That(result, Is.StringContaining(validationResult.ErrorMessage));
            Assert.That(result, Is.StringContaining(validationResult2.ErrorMessage));
        }
    }
}
